import { defineConfig } from '@rspress/core';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const projectRoot = dirname(fileURLToPath(import.meta.url));

export default defineConfig({
  title: 'Breeze IAP',
  description: 'Lightweight async/await wrapper for Unity IAP',
  root: 'docs',
  base: '/breeze-iap/',
  siteOrigin: 'https://docs.somiri.dev',
  outDir: 'out',
  lang: 'ko',
  locales: [
    {
      lang: 'ko',
      label: '한국어',
      title: 'Breeze IAP',
      description: 'Unity IAP를 async/await로 사용하는 경량 결제 래퍼',
    },
    {
      lang: 'en',
      label: 'English',
      title: 'Breeze IAP',
      description: 'A lightweight async/await wrapper for Unity IAP',
    },
    {
      lang: 'ja',
      label: '日本語',
      title: 'Breeze IAP',
      description: 'Unity IAP を async/await で使う軽量な課金ラッパー',
    },
    {
      lang: 'zh',
      label: '简体中文',
      title: 'Breeze IAP',
      description: '基于 async/await 的轻量 Unity IAP 封装',
    },
  ],
  logo: '/logo.svg',
  logoText: 'Breeze IAP',
  icon: '/icon.svg',
  globalStyles: resolve(projectRoot, 'styles/global.css'),
  themeConfig: {
    darkMode: 'force-dark',
  },
});
