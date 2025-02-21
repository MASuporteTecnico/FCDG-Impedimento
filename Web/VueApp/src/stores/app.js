// Utilities
import { defineStore } from 'pinia'

export const useAppStore = defineStore('app', {
  state: () => ({
    UsrOpe : {
      Nome: "RAFAEL BARBOSA SILVA",
      IdEmpresa: 0,
      Empresa: {},
    },
    UsrLogged: false,
    CountDownLogoffReset: false,
    IsLoading: false,
    LastError: "",
    Rotas: [],
    ReadOnly: false,
    ListOnly: false,
    Permissao: {
      SomenteLeitura: false,
      SomenteListar: false
    },
    TituloTela: "",
    MenuLateralATivo: true
  }),
  getters: {
    getUsrOpe: (state) => state.UsrOpe,
    getUsrLogged: (state) => state.UsrLogged,
    getIsLoading: (state) => state.IsLoading,
    GetLastError: (state) => state.LastError,
    GetRotas: (state) => state.Rotas,
    GetReadOnly: (state) => state.ReadOnly,
    GetListOnly: (state) => state.ListOnly,
    GetPermissao: (state) => state.Permissao,
    GetTituloTela: (state) => state.TituloTela,
    GetMenuLateralATivo: (state) => state.MenuLateralATivo,
  },
  actions: {
    UsrLogon() {this.UsrLogged = true;},
    UsrLogoff() {this.UsrLogged = false},
    SetUsrOpe(UsrOpe) {this.UsrOpe = UsrOpe; this.MenuLateralATivo = UsrOpe.MenuLateral;},
    SetIsLoading(status) { this.IsLoading = status },
    SetLastError(status) { this.LastError = status },
    SetRotas(status) { this.Rotas = status },
    SetReadOnly(status) { this.ReadOnly = status },
    SetListOnly(status) { this.ListOnly = status },
    SetPermissao(status) { this.Permissao = status },
    SetTituloTela(status) { this.TituloTela = status },
    SetMenuLateralATivo(status) { this.MenuLateralATivo = status },
  },
  persist: true
})
