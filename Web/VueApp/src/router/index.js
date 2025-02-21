/**
 * router/index.ts
 *
 * Automatic routes for `./src/pages/*.vue`
 */

// Composables
import { createRouter, createWebHistory } from 'vue-router/auto'
import { setupLayouts } from 'virtual:generated-layouts'
import { routes } from 'vue-router/auto-routes'
import { useAppStore } from "@/stores/app";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: setupLayouts(routes),
  scrollBehavior(to, from, savedPosition) {
    return savedPosition || { left: 0, top: 0 }
  }
})

// Workaround for https://github.com/vitejs/vite/issues/11804
router.onError((err, to) => {
  if (err?.message?.includes?.('Failed to fetch dynamically imported module')) {
    if (!localStorage.getItem('vuetify:dynamic-reload')) {
      console.log('Reloading page to fix dynamic import error')
      localStorage.setItem('vuetify:dynamic-reload', 'true')
      location.assign(to.fullPath)
    } else {
      console.error('Dynamic import error, reloading page did not fix it', err)
    }
  } else {
    console.error(err)
  }
})

router.isReady().then(() => {
  localStorage.removeItem('vuetify:dynamic-reload')
})

router.beforeEach((to, from, next) => {

  const store = useAppStore();
  store.CountDownLogoffReset = !store.CountDownLogoffReset;

  const Rotas = store.GetRotas;

  const Rota = Rotas.filter((x) => {
    if ((to.path.indexOf(x.Rota) > -1) && (x.Rota !== '/'))
      return x;
  })

  const publicPages = ['/login', '/logout', '/negado', '/erro'];
  const loggedPublicPages = ['/', '/trocarsenha'];

  const authRequired = !publicPages.includes(to.path.toLowerCase());
  const isLoggedPublicPages = loggedPublicPages.includes(to.path.toLowerCase());
  const loggedIn = store.getUsrLogged;

  document.title = to.meta.title;
  store.SetTituloTela(to.meta.title);

  //Em rotas públicas, libera acesso
  if (publicPages.includes(to.path.toLowerCase())) {
    next();
    return;
  }

  //Em páginas públicas com usuário logado, é liberado
  if (isLoggedPublicPages && loggedIn) {
    next();
    return;
  }

  //Verifica se o usiários está logado.
  if (authRequired && !loggedIn) {
    //Se não estiver logado, redireciona para págian de login
    next('/login');
    return;
  } else {
    //Caso esteja indo para uma rota sem permissão, será redirecionado
    if (Rota.length == 0) {
      next('/negado');
      return;
    } else {

      //Verificar se o acesos é readonly/listonly na rota
      let Permissao = {
        SomenteLeitura : false,
        SomenteListar : false
      }

      if (Rota[0].Permissao >= 7) {
        store.SetReadOnly(false);
        Permissao.SomenteLeitura = false;
      } else {
        store.SetReadOnly(true);
        Permissao.SomenteLeitura = true;

        if (Rota[0].Permissao < 2) {
          store.SetListOnly(true);
          Permissao.SomenteListar = true;
        }
        else {
          store.SetListOnly(false);
          Permissao.SomenteListar = false;
        }
      }

      store.SetPermissao(Permissao);

      next();
      return;
    }
  }
})

export default router
