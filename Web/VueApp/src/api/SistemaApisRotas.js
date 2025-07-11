const SistemaApisRotas = {
  Advogado: {
    Index: "/api/Cadastro/Advogado/Index",
    Edit: "/api/Cadastro/Advogado/Edit",
    Save: "/api/Cadastro/Advogado/Save",
    Delete: "/api/Cadastro/Advogado/Delete",
  },
  Impedimento: {
    Index: "/api/Impedimento/Index",
    Edit: "/api/Impedimento/Edit",
    Save: "/api/Impedimento/Save",
    Delete: "/api/Impedimento/Delete",
  },
  Usuario: {
    Index: "/api/Sistema/Usuario/Index",
    Edit: "/api/Sistema/Usuario/Edit",
    Save: "/api/Sistema/Usuario/Save",
    Delete: "/api/Sistema/Usuario/Delete",
    Menu: "/api/Sistema/Usuario/MenuUsuario",
    MenuLateral: "/api/Sistema/Usuario/MenuLateral",
    Login: "/api/Sistema/Usuario/Login",
    Logout: "/api/Sistema/Usuario/Logout",
    TrocarSenha: "/api/Sistema/Usuario/TrocarSenha",
  },
  Mensagem: {
    Save: "/api/Sistema/Mensagem/Save",
    Edit: "/api/Sistema/Mensagem/Edit",
    SetarLidaNaoLida: "/api/Sistema/Mensagem/SetarLidaNaoLida",
    Delete: "/api/Sistema/Mensagem/Delete",
    Mensagens: "/api/Sistema/Mensagem/Mensagens",
  },
  Grupo: {
    Index: "/api/Sistema/Grupo/Index",
    Edit: "/api/Sistema/Grupo/Edit",
    Save: "/api/Sistema/Grupo/Save",
    Delete: "/api/Sistema/Grupo/Delete",
  },
  Permissao: {
    Index: "/api/Sistema/Permissao/Index",
    Edit: "/api/Sistema/Permissao/Edit",
    Save: "/api/Sistema/Permissao/Save",
    Delete: "/api/Sistema/Permissao/Delete",
  },
  Menu: {
    Edit: "/api/Sistema/Menu/Edit",
    Save: "/api/Sistema/Menu/Save",
  },
  Empresa: {
    Index: "/api/Cadastro/Empresa/Index",
    Edit: "/api/Cadastro/Empresa/Edit",
    Save: "/api/Cadastro/Empresa/Save",
    Delete: "/api/Cadastro/Empresa/Delete",
  },
  Lista: {
    Empresas: "/api/Lista/Empresas",
    Usuarios: "/api/Lista/Usuarios",
    GruposUsuarios: "/api/Lista/GruposUsuarios",
    GruposMenus: "/api/Lista/GruposMenus",
    Menus: "/api/Lista/Menus",
  },
  Parametro: {
    Edit: "/api/Sistema/Parametro/Edit",
    Save: "/api/Sistema/Parametro/Save",
  },
  Auditoria: {
    Index: "/api/Sistema/Auditoria/Index",
  },
  Arquivo: {
    Upload: "/api/Sistema/Arquivo/Upload",
    Download: "/api/Sistema/Arquivo/Download",
  },
};

export default SistemaApisRotas;
