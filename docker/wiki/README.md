# Docker Wiki (Starlight)

This directory is the active documentation site for the project.

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
