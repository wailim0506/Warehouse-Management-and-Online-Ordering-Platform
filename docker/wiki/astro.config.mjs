// @ts-check
import { defineConfig } from "astro/config";
import starlight from "@astrojs/starlight";

import tailwindcss from "@tailwindcss/vite";

// https://astro.build/config
export default defineConfig({
  site: "https://lmcis.mythic3011.com/",

  integrations: [
    starlight({
      title: "LMC Integrated System",
      description:
        "Official technical wiki for the Legend Motor Company Integrated System, including Docker-first setup and user guide documentation.",
      logo: {
        src: "./src/assets/logo.svg",
        alt: "Legend Motor Company Integrated System",
      },
      sidebar: [
        {
          label: "Getting Started",
          items: [
            { label: "System Overview", slug: "" },
            { label: "Installation Roadmap", link: "/installation/" },
            { label: "User Guide Overview", link: "/user-guide/" },
          ],
        },
        {
          label: "Installation",
          items: [
            { label: "Overview", slug: "installation" },
            { label: "Quick Commands", slug: "installation/quick-commands" },
            { label: "Setup", slug: "installation/setup" },
            { label: "API Key", slug: "installation/api-key" },
            { label: "Database", slug: "installation/database" },
            { label: "Configuration", slug: "installation/configuration" },
            { label: "Running", slug: "installation/running" },
            { label: "Services", slug: "installation/services" },
            { label: "Stopping", slug: "installation/stopping" },
            { label: "Troubleshooting", slug: "installation/troubleshooting" },
            {
              label: "Troubleshooting Index",
              slug: "installation/troubleshooting-index",
            },
          ],
        },
        {
          label: "User Guide",
          items: [
            { label: "Overview", slug: "user-guide" },
            {
              label: "Login And System Pages",
              slug: "user-guide/system-pages",
            },
            { label: "Stock Management", slug: "user-guide/stock-management" },
            {
              label: "On-Sale Product Management",
              slug: "user-guide/on-sale-product-management",
            },
            { label: "Order Management", slug: "user-guide/order-management" },
            {
              label: "Invoice Management",
              slug: "user-guide/invoice-management",
            },
            {
              label: "Feedback Management",
              slug: "user-guide/feedback-management",
            },
          ],
        },
      ],
    }),
  ],

  vite: {
    plugins: [tailwindcss()],
  },
});
