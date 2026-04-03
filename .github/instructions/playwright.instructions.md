---
description: 'Playwright E2E test patterns and locator conventions'
applyTo: '**/e2e-tests/**/*.ts'
---

# Playwright E2E Test Instructions

## Test Patterns

### Test Structure

```typescript
import { test, expect } from '@playwright/test';

test.describe('Feature Name', () => {
  test('should do something specific', async ({ page }) => {
    await test.step('Navigate to page', async () => {
      await page.goto('/');
    });

    await test.step('Verify content', async () => {
      const element = page.getByTestId('element-id');
      await expect(element).toBeVisible();
    });
  });
});
```

### Locator Strategies (Priority Order)

1. **`getByTestId`** - For elements with `data-testid`
2. **`getByRole`** - For semantic HTML elements
3. **`getByText`** - For text content
4. **`getByLabel`** - For form elements

### Auto-Retrying Assertions

Always use `await expect()` for assertions:
```typescript
await expect(page.getByTestId('games-grid')).toBeVisible();
await expect(page.getByTestId('game-title')).not.toBeEmpty();
await expect(page).toHaveURL('/game/1');
```

### Important Rules

- **NEVER** use `waitForTimeout` or hard-coded waits
- Use `test.step()` for logical grouping
- Take screenshots only on failure
- Use `data-testid` for all interactive elements

### Available Test IDs

- `games-grid` - Games grid container
- `game-card` - Individual game card
- `game-title` - Game title in card
- `game-category` - Category badge
- `game-publisher` - Publisher badge
- `game-description` - Game description
- `game-details` - Game details container
- `game-details-title` - Game details title
- `game-details-description` - Game details description
- `game-details-category` - Category in details
- `game-details-publisher` - Publisher in details
- `game-rating` - Star rating display
- `back-game-button` - Support This Game button
- `about-section` - About page section
- `about-heading` - About page heading
