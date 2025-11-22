import { defineConfig } from 'vite';
import { angular } from '@angular-devkit/build-angular/plugins/vite';

export default defineConfig({
  plugins: [angular()],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5155',
        changeOrigin: true,
        secure: false
      }
    }
  }
});
