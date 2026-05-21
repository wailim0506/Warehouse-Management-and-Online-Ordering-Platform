# Docker Wiki (Starlight)

This directory is the active documentation site for the project.
It is a documentation product (not an app implementation surface) for:

- Local developers setting up the Docker-backed WinForms environment
- End users and testers following core workflows

Primary docs live under:

- `src/content/docs/installation/**` (Docker-first setup path)
- `src/content/docs/user-guide/**` (task-based feature walkthroughs)
- `src/content/docs/index.mdx` (system overview entry point)

## Local Commands

Run from `docker/wiki`:

- `npm install`
- `npm run dev`
- `npm run build`
- `npm run preview`

## Scope

- Source docs: `src/content/docs/**`
- Static assets: `public/**`
- Site config: `astro.config.mjs`, `src/content.config.ts`

Build artifacts and cache directories (`node_modules`, `.astro`, `dist`) are local-only and must remain untracked.
