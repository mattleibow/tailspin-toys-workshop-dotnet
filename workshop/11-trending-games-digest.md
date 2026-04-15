# Exercise 11 - Trending Games Digest

In the previous exercise you used the ChatOps pattern to trigger a workflow from a GitHub comment. Now you'll build something more powerful: a workflow that reaches out to an **external API** and brings data into your GitHub repository automatically.

**Estimated time:** 20 minutes

## Objectives

- Write a targeted natural-language prompt for an agentic workflow that calls an external API
- Understand frontmatter configuration: network allowlists, tools, and safe outputs
- Inspect and refine the generated workflow markdown
- Validate that the output issue is useful and well-structured

## Scenario

Tailspin Toys is a crowdfunding platform for games. The team wants to stay on top of what's trending in the board game world so they can spot opportunities — new games that might want to crowdfund, or popular titles that could inspire new features. You'll create a workflow that automatically fetches the **BoardGameGeek Hot Games list** every morning and posts it as a GitHub issue.

## Background: The BoardGameGeek API

[BoardGameGeek (BGG)](https://boardgamegeek.com/) is the largest board game database in the world. It provides a public XML API that requires no authentication:

- `GET https://boardgamegeek.com/xmlapi2/hot?type=boardgame` — returns the current "hot" (trending) board games
- `GET https://boardgamegeek.com/xmlapi2/thing?id=<id>&stats=1` — returns details for a specific game (rating, description, etc.)

Your agentic workflow will call these endpoints, parse the results, and format them into a readable GitHub issue.

## Part 1 — Create the workflow

Make sure you are still in the `agentic-workflows` agent. If not, type `/agent` and select it again.

Then provide the following description:

```
Create a daily digest workflow for the Tailspin Toys team that tracks
trending board games. Every weekday, fetch the current hot board games
from the BoardGameGeek XML API
(https://boardgamegeek.com/xmlapi2/hot?type=boardgame). For the top 15
games, include: the game name, its BoardGameGeek rank on the hot list,
the year published, and a link to its BGG page
(https://boardgamegeek.com/boardgame/<id>). Create a GitHub issue
titled "🎲 Trending Games – <date>" with the results formatted as a
Markdown table. Add a brief intro paragraph explaining these are the
currently trending games on BoardGameGeek.

Name the workflow file trending-games.
Allow for manual runs.
```

The agent will confirm the trigger (weekday schedule), required tools (`web-fetch`), permissions (`issues: write`), and the network allowlist (the BGG API domain), then generate the workflow files.

## Part 2 — Review and refine the workflow

Open the generated workflow file:

```bash
cat .github/workflows/trending-games.md
```

The file has two parts:

**YAML frontmatter** (between `---` markers) — configuration that requires recompilation:

```yaml
---
name: Trending Games Digest
on:
  schedule: daily on weekdays
  workflow_dispatch:
permissions:
  issues: write
  contents: read
network:
  - boardgamegeek.com
tools:
  - web-fetch
safe-outputs:
  create-issue:
    max: 1
---
```

**Markdown body** (after the frontmatter) — the plain-English instructions. You can edit the body directly and your changes will take effect on the next run, **without recompiling**.

> [!NOTE]
> If you want to change the trigger, tools, permissions, or network rules (the frontmatter), you need to recompile: `gh aw compile trending-games`.

### Things to check

1. **Network allowlist** — does the frontmatter include `boardgamegeek.com`? The agent needs network access to call the BGG API.
2. **Schedule** — does it use fuzzy scheduling (`daily on weekdays`) rather than a fixed cron? Fuzzy scheduling is preferred because it distributes load and automatically adds `workflow_dispatch` for manual runs.
3. **Prompt body** — does the body clearly describe the filtering criteria and the desired output format?

If you want to adjust the prompt, simply edit the markdown body and the change takes effect on the next run. To update the frontmatter (e.g., add a network domain), edit the frontmatter and recompile:

```bash
gh aw compile trending-games
```

## Part 3 — Open a PR and run the workflow

Stay in the `agentic-workflows` agent and ask it to create a pull request for you:

```
Can you please create a pull request for me!
```

This reuses the same PR skill from earlier in the workshop to commit, push, and open a PR for your workflow files.

Open the PR on GitHub and merge it, then return to your terminal:

```bash
git checkout main
git pull
```

Then trigger a manual run:

```bash
gh aw run trending-games
```

Wait for the run to complete, then open GitHub and check the **Issues** tab.

### What to check

- The issue title contains today's date and the 🎲 emoji.
- The issue body is a Markdown table with game names, hot list ranks, years, and BGG links.
- The games are real, currently trending board games from BoardGameGeek.

> [!TIP]
> If the output looks good but the formatting is off, edit the markdown body of `.github/workflows/trending-games.md` to tweak the output template, then push and re-run — no recompilation needed.

## Success criteria

- [ ] `.github/workflows/trending-games.md` exists in your repository
- [ ] `.github/workflows/trending-games.lock.yml` exists in your repository
- [ ] The workflow frontmatter includes `boardgamegeek.com` in the network allowlist
- [ ] The workflow uses fuzzy scheduling (`daily on weekdays`)
- [ ] A GitHub issue titled **🎲 Trending Games – \<today's date\>** was created with a table of trending board games

## Summary and next steps

You've created a workflow that automatically monitors the board game world for Tailspin Toys. You learned how to:

- write a targeted prompt for an agentic workflow that calls an external API.
- configure network allowlists, tools, and safe outputs in the frontmatter.
- review, refine, compile, and trigger a workflow.
- validate the output issue.

This optional extension shows how Agentic Workflows can also pull live data from external services and turn it into useful updates for your team.

## Resources

- [BoardGameGeek XML API2][bgg-api]
- [Agentic Workflows Reference][aw-reference]

---

[bgg-api]: https://boardgamegeek.com/wiki/page/BGG_XML_API2
[aw-reference]: https://github.github.com/gh-aw/
