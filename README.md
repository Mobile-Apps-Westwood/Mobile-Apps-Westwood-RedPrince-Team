[RedPrince-README.md](https://github.com/user-attachments/files/30625159/RedPrince-README.md)
# RedPrince

A cross-platform casino game app built with .NET MAUI. Play card games against
the house with a persistent account, an in-app balance, and a leaderboard.

Built by a team of five as a mobile apps project.

## Features

| Screen | What it does |
|---|---|
| **Blackjack** | Full game against the dealer — betting with chips, hit/stand/double down, dealer play, and win/loss/push tracking |
| **Baccarat** | Second card game |
| **Accounts** | Create an account, sign in, change username or password |
| **Store** | Spend your in-app balance |
| **Leaderboard** | Compare balances across accounts |
| **Hint** | Password hint recovery |
| **Settings** | Account management |

Accounts and balances persist locally in SQLite, so your money is still there
next time you open the app.

## Tech stack

- **.NET 10 / C#**
- **.NET MAUI** — one codebase, four targets
- **MVVM** via [CommunityToolkit.Mvvm](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/) — `ObservableObject`, `ICommand`, and observable collections binding straight to XAML
- **SQLite** via `sqlite-net-pcl` for local persistence
- **Shell navigation** (`AppShell.xaml`) for routing between pages

### Platform targets

| Platform | Minimum version |
|---|---|
| Android | 21 (Lollipop) |
| iOS | 15.0 |
| macOS (Mac Catalyst) | 15.0 |
| Windows | 10.0.17763 |

## Project layout

```
RedPrince/
├── Models/          # Data models — User, Card, Deck, Hand, GameResults
├── ViewModels/      # One view model per screen; game logic lives here
├── Views/           # XAML pages
├── Services/        # DatabaseService (SQLite access)
├── Platforms/       # Per-platform entry points and manifests
└── Resources/       # Images, fonts, styles
```

The card models (`Deck`, `Hand`, `Card`) are game-agnostic, so both card games
draw from the same shuffling and hand-evaluation code.

## Running it

You'll need the [.NET 10 SDK](https://dotnet.microsoft.com/download) and the
MAUI workload:

```bash
dotnet workload install maui
```

Then from the repo root:

```bash
# Android
dotnet build RedPrince/RedPrince.csproj -t:Run -f net10.0-android

# Windows
dotnet build RedPrince/RedPrince.csproj -t:Run -f net10.0-windows10.0.19041.0
```

iOS and Mac Catalyst builds require macOS with Xcode installed.

Opening `RedPrince.slnx` in Visual Studio 2022+ also works — pick a target from
the debug dropdown and hit run.

## Who built what

| Contributor | Area |
|---|---|
| [@Blairqiao](https://github.com/Blairqiao) | Project structure, database service, app-wide refactoring |
| [@whatisabadname](https://github.com/whatisabadname) | Blackjack — deck/hand/card models, game engine, view model, and navigation integration |

## Known limitations

Worth being upfront about, since this was a learning project:

- **Passwords are stored in plaintext** in the local SQLite database. A real app
  would hash and salt them (bcrypt, Argon2, or ASP.NET Core Identity's hasher).
- Accounts are **local to the device** — there's no server, so the leaderboard
  only ranks accounts created on that install.
- No automated tests.
