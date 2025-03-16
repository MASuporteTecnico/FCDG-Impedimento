// Utilities
import { defineStore } from 'pinia'

export const useAppStore = defineStore('app', {
  state: () => ({
    UsrOpe : {
      Nome: "",
      IdEmpresa: 0,
      Empresa: {},
    },
    UsrLogged: false,
    CountDownLogoffReset: false,
    IsLoading: false,
    LastError: "",
    Rotas: [],
    Permissao: {
      SomenteLeitura: false,
      SomenteListar: false
    },
    TituloTela: "",
    MenuLateralATivo: true,
    ErroSistema: "",
    Dominio: null
  }),
  getters: {
    getUsrOpe: (state) => state.UsrOpe,
    getUsrLogged: (state) => state.UsrLogged,
    getIsLoading: (state) => state.IsLoading,
    GetLastError: (state) => state.LastError,
    GetRotas: (state) => state.Rotas,
    GetPermissao: (state) => state.Permissao,
    GetTituloTela: (state) => state.TituloTela,
    GetMenuLateralATivo: (state) => state.MenuLateralATivo,
    GetErroSistema: (state) => state.ErroSistema,
    GetDominio: (state) => state.Dominio
  },
  actions: {
    UsrLogon() {this.UsrLogged = true;},
    UsrLogoff() {this.UsrLogged = false},
    SetUsrOpe(UsrOpe) {this.UsrOpe = UsrOpe; this.MenuLateralATivo = UsrOpe.MenuLateral;},
    SetIsLoading(status) { this.IsLoading = status },
    SetLastError(status) { this.LastError = status },
    SetRotas(status) { this.Rotas = status },
    SetPermissao(status) { this.Permissao = status },
    SetTituloTela(status) { this.TituloTela = status },
    SetMenuLateralATivo(status) { this.MenuLateralATivo = status },
    SetErroSistema(status) { this.ErroSistema = status },
    SetDominio(status) { this.Dominio = status }
  },
  persist: true
})
