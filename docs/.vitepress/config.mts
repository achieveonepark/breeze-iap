import { defineConfig } from 'vitepress'

export default defineConfig({
  title: 'Breeze IAP',
  description: 'Async/await Unity IAP wrapper — Unity IAP 5.0.0+',
  base: '/breeze-iap/',

  themeConfig: {
    nav: [
      { text: 'Guide',     link: '/getting-started' },
      { text: 'API',       link: '/api/initialize' },
      { text: 'Changelog', link: '/changelog' },
    ],

    sidebar: [
      {
        text: 'Guide',
        items: [
          { text: 'Introduction',   link: '/' },
          { text: 'Getting Started', link: '/getting-started' },
        ],
      },
      {
        text: 'API Reference',
        items: [
          { text: 'InitializeAsync', link: '/api/initialize' },
          { text: 'PurchaseAsync',   link: '/api/purchase' },
          { text: 'Confirm',         link: '/api/confirm' },
          { text: 'Restore',         link: '/api/restore' },
          { text: 'GetPendingList',  link: '/api/pending' },
          { text: 'Types',           link: '/api/types' },
        ],
      },
      {
        text: 'More',
        items: [
          { text: 'Changelog', link: '/changelog' },
        ],
      },
    ],

    socialLinks: [
      { icon: 'github', link: 'https://github.com/achieveonepark/breeze-iap' },
    ],

    footer: {
      message: 'Released under the MIT License.',
    },

    search: {
      provider: 'local',
    },
  },
})
