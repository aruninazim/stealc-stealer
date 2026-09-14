# stealc-stealer

> compact · exfil

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dot.net)
[![Stealer](https://img.shields.io/badge/type-stealer-red)]()
[![Infostealer](https://img.shields.io/badge/family-infostealer-orange)]()
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Stealc compact layout — minimal stub, full grab path list, fast exfil.

Family name plus stealer suffix.

Lab / research build. All I/O is simulated — no live data exfiltration in default configuration.

---

## Target coverage

### Browsers — Chromium

Google Chrome, Microsoft Edge, Brave, Opera, Opera GX, Vivaldi, Yandex Browser, Chromium,
Epic Privacy Browser, CentBrowser, 7Star, Iridium, Comodo Dragon, Torch, Amigo, Sputnik, Slimjet.

Collected: cookies, saved logins, autofill, credit cards, bookmarks, history, extension list.

### Browsers — Gecko

Mozilla Firefox, Waterfox, Pale Moon, LibreWolf, Thunderbird.

Collected: cookies, logins.json / key4.db, bookmarks, history.

### Crypto wallets

Desktop: Exodus, Electrum, Atomic, Jaxx, Coinomi, Guarda, Wasabi, Bitcoin Core, Litecoin Core,
Dash Core, Monero GUI, Ledger Live, Binance Desktop.

Extensions: MetaMask, Phantom, Ronin, Coinbase Wallet, Trust Wallet, TronLink, Solflare, Keplr, Rabby, OKX.

### Messengers

Discord (+ Canary, PTB), Telegram Desktop, Signal, Skype, Slack, Microsoft Teams, Element.

### System

Hostname, username, OS, HWID, locale, timezone, screen resolution, installed software, process list.

---

## Build

```bash
dotnet restore stealc-stealer.slnx
dotnet build stealc-stealer.slnx -c Release
dotnet test stealc-stealer.slnx -c Release
```

## CLI

```bash
dotnet run --project src/stealc-stealer.Agent -- harvest
dotnet run --project src/stealc-stealer.Agent -- scan
dotnet run --project src/stealc-stealer.Agent -- status
dotnet run --project src/stealc-stealer.Agent -- anti
```

| Command | Description |
|---------|-------------|
| `harvest` | Run full grab pipeline (lab mode) |
| `scan` | Enumerate all target paths |
| `status` | Print system fingerprint |
| `anti` | Run anti-analysis checks |

## Project structure

```
stealc-stealer/
├── src/
│   ├── stealc-stealer.Grabber/
│   │   ├── Grabbers/        # browser, wallet, messenger, system
│   │   ├── Parsers/         # chromium, gecko, cookie decryptor
│   │   ├── Exfil/           # log builder, zip packer, fingerprint
│   │   └── Core/            # pipeline orchestrator, anti-analysis
│   └── stealc-stealer.Agent/        # CLI entry point
└── tests/
    └── stealc-stealer.Grabber.Tests/
```

## Anti-analysis checks

Sandbox username list, VM process detection, debugger attach, low-resource machine, fresh boot (uptime < 2 min).

## IOC reference

All file paths, registry keys, and token formats documented in source code for SOC/IR training.

## License

MIT — Copyright (c) 2026


---

## Topics

![stealc](https://img.shields.io/badge/stealc-111827?style=flat-square) ![stealer](https://img.shields.io/badge/stealer-111827?style=flat-square) ![infostealer](https://img.shields.io/badge/infostealer-111827?style=flat-square) ![malware](https://img.shields.io/badge/malware-111827?style=flat-square) ![malware-analysis](https://img.shields.io/badge/malware%20analysis-111827?style=flat-square) ![compact](https://img.shields.io/badge/compact-111827?style=flat-square) ![security-research](https://img.shields.io/badge/security%20research-111827?style=flat-square) ![csharp](https://img.shields.io/badge/csharp-111827?style=flat-square)

`stealc` `stealer` `infostealer` `malware` `malware-analysis` `compact` `security-research` `csharp`

Search: stealc-stealer · compact · exfil · StealC compact stealer — minimal footprint, full grab paths, fast exfil schema

---

<sub>StealC compact stealer — minimal footprint, full grab paths, fast exfil schema</sub>
