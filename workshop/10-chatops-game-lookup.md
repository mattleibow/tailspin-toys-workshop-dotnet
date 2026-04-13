# Exercise 10 - ChatOps: Game Lookup Slash Command

In the previous exercises you built scheduled workflows that run automatically. Now you'll use the **ChatOps pattern** to create an interactive slash command that team members can trigger directly from a GitHub issue comment.

**Estimated time:** 20 minutes

## Objectives

- Understand the ChatOps pattern for agentic workflows
- Create a `/game-lookup` slash command triggered by an issue comment
- Have the agent search BoardGameGeek, fetch game details, and reply inline
- Test the command end-to-end in a real GitHub issue

## Background: The ChatOps pattern

The ChatOps pattern lets team members trigger agentic workflows by posting a **slash command** in a GitHub issue or pull request comment. The pattern works like this:

1. A user posts a comment containing a slash command and arguments (e.g., `/game-lookup Catan`).
2. GitHub fires an `issue_comment` webhook event.
3. The agentic workflow detects the slash command, extracts the arguments, runs its logic, and posts the result as a reply in the same thread.

This is powerful because it keeps context inside GitHub — no need to switch to a separate tool. For the Tailspin Toys team, this means anyone can quickly look up a board game's details without leaving the issue they're discussing.

## Scenario

The Tailspin Toys team is evaluating new games to feature on their crowdfunding platform. During discussions in GitHub issues, team members frequently want to look up a game's rating, year of publication, and description. Instead of switching to a browser, you'll create a ChatOps command that does the lookup right from the issue thread.

## Part 1 — Create the slash command workflow

Create the workflow with `gh aw new`:

```bash
gh aw new game-lookup
```

When the interactive session opens, describe what you want:

```
Create a ChatOps slash command called /game-lookup. When a user posts
a comment on a GitHub issue that starts with "/game-lookup <name>",
where <name> is a board game name, do the following:
1) Search for the game on BoardGameGeek using the XML API:
   https://boardgamegeek.com/xmlapi2/search?query=<name>&type=boardgame&exact=1
   If no exact match is found, retry without exact=1 to get fuzzy results.
2) Take the first result and fetch its details from:
   https://boardgamegeek.com/xmlapi2/thing?id=<id>&stats=1
3) Extract: the game name, year published, average rating, number of
   ratings, minimum and maximum players, playing time, and a short
   description (first 200 characters).
4) Reply to the original issue comment with the game details formatted
   in Markdown, including a link to the BGG page.
If no game name is provided or no results are found, reply with a
helpful error message.
```

The agent will configure:
- **Trigger**: `issue_comment` with a condition filtering for comments starting with `/game-lookup`
- **Tools**: `web-fetch` to call the BoardGameGeek API
- **Network**: `boardgamegeek.com` in the allowlist
- **Safe outputs**: `add-comment` to reply to the issue

## Part 2 — Review the generated workflow

Open the generated file:

```bash
cat .github/workflows/game-lookup.md
```

The frontmatter will look similar to:

```yaml
---
name: Game Lookup
on:
  issue_comment:
    types: [created]
    if: startsWith(github.event.comment.body, '/game-lookup')
permissions:
  issues: write
  contents: read
network:
  - boardgamegeek.com
tools:
  - web-fetch
safe-outputs:
  add-comment:
    max: 1
---
```

### Things to verify

1. **Trigger condition** — the `if:` clause filters so the workflow only runs when someone types `/game-lookup`, not on every comment.
2. **Network allowlist** — `boardgamegeek.com` must be listed so the agent can search and fetch game details.
3. **Safe output** — `add-comment` is declared so the agent can post the result back to the issue.
4. **Prompt body** — the markdown body describes how to parse the game name, call the API, and format the reply.

> [!TIP]
> You can edit the markdown body directly (on GitHub.com or locally) without recompiling. For example, you can tweak the output template or adjust what details are included.

## Part 3 — Compile, commit, and push

If the agent did not compile the workflow automatically, compile it now:

```bash
gh aw compile game-lookup
```

Commit and push both the markdown and the lock file:

```bash
git add .github/workflows/game-lookup.md .github/workflows/game-lookup.lock.yml
git commit -m "Add game-lookup ChatOps slash command"
git push
```

> [!IMPORTANT]
> The `issue_comment` trigger only fires when the workflow lock file is on the **default branch** of the repository (usually `main`). Make sure you push to `main` (or merge a PR into `main`) before testing.

## Part 4 — Test the slash command

1. Open any issue in your repository (or create a new one titled "Testing ChatOps").
2. Post a comment with a game name, for example:

    ```
    /game-lookup Catan
    ```

3. Wait 20–60 seconds. The agentic workflow will run in the background.
4. Refresh the issue page. You should see a new comment from the agent containing game details.

### Example expected output

```markdown
## 🎲 Game Lookup: Catan

| Detail | Value |
|--------|-------|
| **Name** | Catan |
| **Year** | 1995 |
| **Rating** | 7.1 / 10 (95,000+ ratings) |
| **Players** | 3–4 |
| **Playing Time** | 60–120 min |

**Description:** In Catan, players try to be the dominant force on the
island of Catan by building settlements, cities, and roads…

🔗 [View on BoardGameGeek](https://boardgamegeek.com/boardgame/13/catan)
```

> [!TIP]
> Try looking up different games! Some good ones to test: `Wingspan`, `Ticket to Ride`, `Pandemic`, `Azul`.

## Troubleshooting

| Problem | Solution |
|---------|---------|
| Agent doesn't reply | Check that the lock file is on the default branch and that the trigger condition matches. |
| "No results found" | The game name might need to be more specific. Try the full official name. |
| Permission denied | Ensure `issues: write` is in the workflow frontmatter and the lock file is recompiled. |
| Reply is empty | The BGG API may have rate-limited the request. Wait a moment and try again. |

## Success criteria

- [ ] `.github/workflows/game-lookup.md` exists and is pushed to the default branch
- [ ] `.github/workflows/game-lookup.lock.yml` exists and is pushed to the default branch
- [ ] Posting `/game-lookup Catan` in a GitHub issue triggers the workflow
- [ ] The agent replies with game details (name, rating, year, players, etc.)
- [ ] The reply includes a link to the BoardGameGeek page

## Summary and next steps

You've built a ChatOps slash command that lets the Tailspin Toys team look up board game details without leaving GitHub. You learned how to:

- use the ChatOps pattern with `issue_comment` triggers.
- parse arguments from a slash command.
- call an external API and format the results.
- use `add-comment` safe outputs to reply inline.

## Resources

- [ChatOps Pattern Reference][chatops-pattern]
- [BoardGameGeek XML API2][bgg-api]
- [Agentic Workflows Reference][aw-reference]

---

[chatops-pattern]: https://github.github.com/gh-aw/patterns/chat-ops/
[bgg-api]: https://boardgamegeek.com/wiki/page/BGG_XML_API2
[aw-reference]: https://github.github.com/gh-aw/
