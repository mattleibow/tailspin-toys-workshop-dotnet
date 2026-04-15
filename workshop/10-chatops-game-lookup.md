# Exercise 10 - ChatOps: Game Lookup Slash Command

In the previous exercises you built scheduled workflows that run automatically. Now you'll use the **ChatOps pattern** to create an interactive slash command that team members can trigger directly from a GitHub issue comment.

**Estimated time:** 20 minutes

## Objectives

- Understand the ChatOps pattern for agentic workflows
- Create a `/game-lookup` slash command triggered by an issue comment
- Have the agent search FreeToGame, fetch game details, and reply inline
- Test the command end-to-end in a real GitHub issue

## Background: The ChatOps pattern

The ChatOps pattern lets team members trigger agentic workflows by posting a **slash command** in a GitHub issue or pull request comment. The pattern works like this:

1. A user posts a comment containing a slash command and arguments (e.g., `/game-lookup Warframe`).
2. GitHub fires an `issue_comment` webhook event.
3. The agentic workflow detects the slash command, extracts the arguments, runs its logic, and posts the result as a reply in the same thread.

This is powerful because it keeps context inside GitHub — no need to switch to a separate tool. For the Tailspin Toys team, this means anyone can quickly look up a game's details without leaving the issue they're discussing.

## Scenario

The Tailspin Toys team is evaluating new games to feature on their crowdfunding platform. During discussions in GitHub issues, team members frequently want to look up a game's genre, platform, publisher, and description. Instead of switching to a browser, you'll create a ChatOps command that does the lookup right from the issue thread.

## Part 1 — Create the slash command workflow

Make sure you are still in the `agentic-workflows` agent. If not, type `/agent` and select it again.

Then describe what you want:

```
Create a ChatOps slash command called /game-lookup. When a user posts
a comment on a GitHub issue that starts with "/game-lookup <name>",
where <name> is a game title, do the following:
1) Fetch the full game list from the FreeToGame API:
   https://www.freetogame.com/api/games
2) Find the best title match in the returned JSON, preferring an exact
   case-insensitive title match and falling back to a fuzzy contains match.
3) Take the first matching result and fetch its details from:
   https://www.freetogame.com/api/game?id=<id>
4) Extract: the game name, genre, platform, publisher, developer,
   release date, and a short description (first 200 characters).
5) Reply to the original issue comment with the game details formatted
   in Markdown, including a link to the FreeToGame page.
If no game name is provided or no results are found, reply with a
helpful error message.

Name the workflow file game-lookup.
```

The agent will configure:
- **Trigger**: `issue_comment` with a condition filtering for comments starting with `/game-lookup`
- **Tools**: `web-fetch` to call the FreeToGame API
- **Network**: `www.freetogame.com` in the allowlist
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
  - www.freetogame.com
tools:
  - web-fetch
safe-outputs:
  add-comment:
    max: 1
---
```

### Things to verify

1. **Trigger condition** — the `if:` clause filters so the workflow only runs when someone types `/game-lookup`, not on every comment.
2. **Network allowlist** — `www.freetogame.com` must be listed so the agent can fetch game details.
3. **Safe output** — `add-comment` is declared so the agent can post the result back to the issue.
4. **Prompt body** — the markdown body describes how to parse the game name, call the API, and format the reply.

> [!TIP]
> You can edit the markdown body directly (on GitHub.com or locally) without recompiling. For example, you can tweak the output template or adjust what details are included.

## Part 3 — Open a PR and merge it

If the agent did not compile the workflow automatically, compile it now:

```bash
gh aw compile game-lookup
```

Then, while still in the `agentic-workflows` agent, ask Copilot to create a pull request:

```
Can you please create a pull request for me!
```

This reuses the same PR skill from earlier in the workshop to commit, push, and open a PR for your workflow files.

Open the PR on GitHub and merge it, then switch back to `main`:

```bash
git checkout main
git pull
```

> [!IMPORTANT]
> The `issue_comment` trigger only fires when the workflow lock file is on the **default branch** of the repository (usually `main`). Make sure you push to `main` (or merge a PR into `main`) before testing.

## Part 4 — Test the slash command

1. Open any issue in your repository (or create a new one titled "Testing ChatOps").
2. Post a comment with a game name, for example:

    ```
    /game-lookup Warframe
    ```

3. Wait 20–60 seconds. The agentic workflow will run in the background.
4. Refresh the issue page. You should see a new comment from the agent containing game details.

### Example expected output

```markdown
## 🎮 Game Lookup: Warframe

| Detail | Value |
|--------|-------|
| **Name** | Warframe |
| **Genre** | Shooter |
| **Platform** | PC (Windows) |
| **Publisher** | Digital Extremes |
| **Release Date** | 2013-03-25 |

**Description:** A cooperative free-to-play action game with fast combat,
space-ninja movement, and a huge amount of progression content.

🔗 [View on FreeToGame](https://www.freetogame.com/open/warframe)
```

> [!TIP]
> Try looking up different games! Some good ones to test: `Warframe`, `Paladins`, `Path of Exile`, `SMITE`.

## Troubleshooting

| Problem | Solution |
|---------|---------|
| Agent doesn't reply | Check that the lock file is on the default branch and that the trigger condition matches. |
| "No results found" | The game name might need to be more specific. Try the full official name. |
| Permission denied | Ensure `issues: write` is in the workflow frontmatter and the lock file is recompiled. |
| Reply is empty | The API response may not have been parsed correctly. Re-check the prompt instructions and try again. |

## Success criteria

- [ ] `.github/workflows/game-lookup.md` exists and is pushed to the default branch
- [ ] `.github/workflows/game-lookup.lock.yml` exists and is pushed to the default branch
- [ ] Posting `/game-lookup Warframe` in a GitHub issue triggers the workflow
- [ ] The agent replies with game details (name, genre, platform, publisher, etc.)
- [ ] The reply includes a link to the FreeToGame page

## Summary and next steps

You've built a ChatOps slash command that lets the Tailspin Toys team look up game details without leaving GitHub. You learned how to:

- use the ChatOps pattern with `issue_comment` triggers.
- parse arguments from a slash command.
- call an external API and format the results.
- use `add-comment` safe outputs to reply inline.

If you have more time, the next exercise shows how to build a scheduled digest that pulls popular games from an external API.

## Resources

- [ChatOps Pattern Reference][chatops-pattern]
- [FreeToGame API Docs][freetogame-api]
- [Agentic Workflows Reference][aw-reference]

---

[chatops-pattern]: https://github.github.com/gh-aw/patterns/chat-ops/
[freetogame-api]: https://www.freetogame.com/api-doc
[aw-reference]: https://github.github.com/gh-aw/
