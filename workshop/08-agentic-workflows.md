# Exercise 8 - Automating Your Repo with Agentic Workflows

Over the previous exercises you've explored GitHub Copilot CLI from multiple angles — custom instructions, MCP servers, code generation, agent skills, custom agents, and slash commands. You've built real features for Tailspin Toys, created pull requests, and learned how to get the most out of AI-assisted development.

Now let's take things further. Instead of running Copilot from your terminal one prompt at a time, what if you could describe an automation in plain English and have it run on a schedule or in response to events — right inside GitHub Actions? That's exactly what **Agentic Workflows** enable.

In this exercise, you will:

- learn what agentic workflows are and why they complement the skills you've already built.
- install the `gh aw` CLI extension and initialise it in your repository.
- create your first agentic workflow: a **daily digest of open issues and pull requests** for the Tailspin Toys project.
- trigger the workflow and verify it creates a summary issue.

## What are Agentic Workflows?

Agentic Workflows let you describe *what* you want done in plain English. Copilot figures out *how* to do it, runs the necessary tools, and posts the results back to GitHub. Each workflow is a **markdown file** with a small YAML frontmatter block at the top. The frontmatter declares things like the trigger, required tools, and permissions. The markdown body is a plain-English prompt.

Before a workflow can run in GitHub Actions, it must be **compiled** into a lock file. You commit both the `.md` (human-readable) and the `.lock.yml` (machine-readable) files.

```
.github/workflows/
├── daily-digest.md          ← you write this (plain English)
└── daily-digest.lock.yml    ← compiled (auto-generated)
```

## Scenario

You've been building features for Tailspin Toys across the previous exercises. Your repository now has issues, branches, and pull requests. The team wants a way to stay on top of all this activity without manually checking GitHub every morning. You'll create an agentic workflow that generates a daily digest — automatically.

## Install the Agentic Workflows extension

The `gh aw` CLI extension installs through the standard GitHub CLI extension mechanism, which works across platforms.

> [!NOTE]
> This step requires the **GitHub CLI (`gh`)**. If you set it up during the prerequisites, you're good to go. If not, install it now from [cli.github.com](https://cli.github.com/) and authenticate with `gh auth login`.

1. Return to your codespace or terminal.
2. Install the `gh aw` extension:

    ```bash
    gh extension install github/gh-aw
    ```

3. Verify the extension is available:

    ```bash
    gh aw version
    ```

> [!TIP]
> If `gh aw version` returns "unknown command", verify GitHub CLI is installed with `gh --version`, then re-run `gh extension install github/gh-aw`.

## Initialise Agentic Workflows in your repository

From the root of your Tailspin Toys repository, run:

```bash
gh aw init
```

This sets up your repository for agentic workflows. It creates several files, including:

- `.gitattributes` — marks compiled lock files as generated
- `.github/agents/agentic-workflows.agent.md` — an AI assistant for creating and editing workflows
- `.vscode/settings.json` and `.vscode/mcp.json` — editor configuration

After `gh aw init`, you can open the installed agent file to see the metadata that powers the workflow authoring experience:

```bash
cat .github/agents/agentic-workflows.agent.md
```

It includes frontmatter like:

```yaml
---
description: GitHub Agentic Workflows (gh-aw) - Create, debug, and upgrade AI-powered workflows with intelligent prompt routing
disable-model-invocation: true
---
```

## Create a daily digest workflow

Now create your first workflow — a daily digest of all open issues and pull requests in the Tailspin Toys repository.

1. Start Copilot CLI:

    ```bash
    copilot
    ```

2. Type `/agent` and press Enter.

3. Select the `agentic-workflows` agent.

4. Paste in this prompt:

    ```
    Every weekday, create a GitHub issue that summarises all open issues
    and pull requests in this repository. Group them by label. Include the
    total count, the title, the author, and how long each item has been
    open. Title the issue "Daily Digest – <date>".

    Name the workflow file daily-digest.
    Allow for manual runs.
    ```

5. The agent will ask clarifying questions (such as what trigger to use and whether write permissions are needed) and then generate the workflow files for you.

6. If Copilot asks you to create a `digest` label for the generated issue, do this:

   1. Open your repository on GitHub and go to **Issues** > **Labels**.
   2. Click **New label**.
   3. Enter `digest`, choose any colour you like, create the label, and then return to Copilot.

### What gets created

After the agent finishes, you will have:

- `.github/workflows/daily-digest.md` — the human-readable workflow
- `.github/workflows/daily-digest.lock.yml` — the compiled file for GitHub Actions

Open the markdown file to see what the agent wrote:

```bash
cat .github/workflows/daily-digest.md
```

The frontmatter will look similar to:

```yaml
---
on:
  schedule:
    - cron: "0 8 * * 1-5"
  workflow_dispatch:
permissions:
  issues: read
  pull-requests: read
safe-outputs:
  mentions: false
  allowed-github-references: []
  create-issue:
    title-prefix: "Daily Digest –"
    labels: [digest]
    close-older-issues: true
    expires: 7
---
```

And the body is a plain-English description of what the agent should do.

> [!NOTE]
> Agentic workflow files are regular markdown — commit them to version control just like any other code. The `.lock.yml` is auto-generated and should not be edited by hand.

## Trigger the workflow

Now use the default Copilot agent to package the generated workflow into a pull request:

1. Type `/agent` and press Enter.
2. Select the default agent.
3. Enter this prompt:

   ```
   Can you please create a pull request for me!
   ```

This reuses the same `branches-commits-prs` skill from earlier in the workshop to commit, push, and open a PR for your workflow files.

4. Open the PR on GitHub and merge it.

5. Return to your terminal and switch back to `main`:

```bash
git checkout main
git pull
```

Once the workflow is on `main`, trigger a manual run using either of these options:

### Option 1: Run it from the CLI

```bash
gh aw run daily-digest
```

### Option 2: Run it from the GitHub UI

1. Open your repository on GitHub and go to the **Actions** tab.
2. Select the `daily-digest` workflow from the list on the left.
3. Click **Run workflow**.
4. Choose the `main` branch and confirm the run.

After the run completes (usually under a minute), open GitHub and check the **Issues** tab. You should see a new issue titled **Daily Digest – \<today's date\>** summarising your open issues and PRs — including the backlog items you created in Exercise 3!

> [!NOTE]
> If the repository has few open issues or PRs, the digest will reflect that. The workflow is still working correctly.

## Summary and next steps

You've transitioned from building features manually with Copilot CLI to automating your development workflow. In this exercise you:

- learned what agentic workflows are and how they work as markdown + compiled lock files.
- installed the `gh aw` CLI extension and initialised it in your repository.
- created a daily digest workflow that summarises your Tailspin Toys issues and PRs.
- triggered and verified the workflow end-to-end.

In the next exercises, you'll create more sophisticated workflows that pull data from external APIs and respond to team actions in real time.

## Resources

- [Agentic Workflows Quick Start][aw-quickstart]
- [GitHub CLI][gh-cli]
- [About GitHub Actions][github-actions]

---

[aw-quickstart]: https://github.github.com/gh-aw/setup/quick-start/
[gh-cli]: https://cli.github.com/
[github-actions]: https://docs.github.com/actions
