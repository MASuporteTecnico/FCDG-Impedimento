import axios from 'axios';
import SistemaApisRotas from './SistemaApisRotas';
import Router from '../router'; 
import { useAppStore } from "@/stores/app";

//Pare requsições que não disparam o Loading
let Axios = axios.create();

//AXIOS INTERCEPTORS PARA RESPONSE
Axios.interceptors.response.use(
  (response) => {

    return response;
  },
  (error) => {

    const store = useAppStore();

    switch (error.status) {
      case 401:
        Router.push({ name: "/Logout" });
        break;

      case 403:
        Router.push({ name: "/Negado" });
        break;

      case 500:
        msg = error.response.data?.Mensagem || "Erro interno do servidor (500).";
        store.SetErroSistema(msg);
        Router.push({ name: "/Erro" });
        break;

      default:
        // ---
        break;
    }
    return Promise.reject(error);
  }
);

async function Index(api, paginacao) {
  try {
    const response = await axios.post(`${api}`, paginacao);
    return response.data;
  } catch (error) {
    throw new Error(`${error.response?.data.Mensagem || error.message}`);
  }
}

async function Edit(api, id) {
  try {
    const response = await axios.get(`${api}/${id}`);
    return response.data;
  } catch (error) {
    throw new Error(`${error.response?.data.Mensagem || error.message}`);
  }
}

async function Save(api,dados) {
  try {
    const response = await axios.put(`${api}`, dados);
    return response.data;
  } catch (error) {
    throw new Error(`${error.response?.data.Mensagem || error.message}`);
  }
}

async function Delete(api,dados) {
  try {
    const response = await axios.delete(`${api}`, {data:dados});
    return response.data;
  } catch (error) {
    throw new Error(`${error.response?.data.Mensagem || error.message}`);
  }
}

async function FileUpload(api, dados) {
  try {
    const header = { "Content-type": "multipart/form-data" };
    const response = await axios.put(`${api}`, dados, {headers: header});
    return response.data;
  } catch (error) {
    throw new Error(`${error.response?.data.Mensagem || error.message}`);
  }
}


async function GetLista(api) {
  try {
    const response = await axios.get(`${api}`);
    return response.data;
  } catch (error) {
    throw new Error(`${error.response?.data.Mensagem || error.message}`);
  }
}

async function Get(api) {
  try {
    const response = await Axios.get(`${api}`);
    return response.data;
  } catch (error) {
    throw new Error(`${error.response?.data.Mensagem || error.message}`);
  }
}

const SistemaApis = {
  Rotas: SistemaApisRotas,
  Usuario: {
    async Index(paginacao) {
      return await Index(`${SistemaApisRotas.Usuario.Index}`, paginacao);
    },

    async Edit(id) {
      return await Edit(`${SistemaApisRotas.Usuario.Edit}`,`${id}`);
    },

    async Save(dados) {
      return await Save(`${SistemaApisRotas.Usuario.Save}`, dados);
    },

    async Delete(dados) {
      return await Delete(`${SistemaApisRotas.Usuario.Delete}`, dados);
    },

    async Menu() {
      return await Get(`${SistemaApisRotas.Usuario.Menu}`);
    },

    async MenuLateral(ativo) {
      return await Get(`${SistemaApisRotas.Usuario.MenuLateral}/${ativo}`);
    },

    async Login(usuario) {
      try {
        const response = await axios.post(SistemaApisRotas.Usuario.Login, usuario);

        if (response.Sucesso == false) {
          throw new Error(response.Mensagem);
        }

        return response.data;
      } catch (error) {
        throw new Error(`${error.response?.data.Mensagem || error.message}`);
      }
    },

    async Logout() {
      try {
        const response = await axios.post(SistemaApisRotas.Usuario.Logout);

        if (response.Sucesso == false) {
          throw new Error(response.Mensagem);
        }

        return response.data;
      } catch (error) {
        throw new Error(`${error.response?.data.Mensagem || error.message}`);
      }
    },

    async TrocarSenha(dados) {
      return await Save(`${SistemaApisRotas.Usuario.TrocarSenha}`, dados);
    },
  },

  Grupo: {
    async Index(paginacao) {
      return await Index(`${SistemaApisRotas.Grupo.Index}`, paginacao);
    },

    async Edit(id) {
      return await Edit(`${SistemaApisRotas.Grupo.Edit}`, `${id}`);
    },

    async Save(dados) {
      return await Save(`${SistemaApisRotas.Grupo.Save}`, dados);
    },

    async Delete(dados) {
      return await Delete(`${SistemaApisRotas.Grupo.Delete}`, dados);
    },
  },

  Permissao: {
    async Index(paginacao) {
      return await Index(`${SistemaApisRotas.Permissao.Index}`, paginacao);
    },

    async Edit(id) {
      return await Edit(`${SistemaApisRotas.Permissao.Edit}`, `${id}`);
    },

    async Save(dados) {
      return await Save(`${SistemaApisRotas.Permissao.Save}`, dados);
    },

    async Delete(dados) {
      return await Delete(`${SistemaApisRotas.Permissao.Delete}`, dados);
    },
  },

  Menu: {
    async Edit(id) {
      return await Edit(`${SistemaApisRotas.Menu.Edit}`, `${id}`);
    },

    async Save(dados) {
      return await Save(`${SistemaApisRotas.Menu.Save}`, dados);
    },
  },

  Empresa: {
    async Index(paginacao) {
      return await Index(`${SistemaApisRotas.Empresa.Index}`, paginacao);
    },

    async Edit(id) {
      return await Edit(`${SistemaApisRotas.Empresa.Edit}`, `${id}`);
    },

    async Save(dados) {
      return await Save(`${SistemaApisRotas.Empresa.Save}`, dados);
    },

    async Delete(dados) {
      return await Delete(`${SistemaApisRotas.Empresa.Delete}`, dados);
    },
  },

  Lista: {
    async Empresas() {
      return await GetLista(`${SistemaApisRotas.Lista.Empresas}`);
    },

    async Usuarios() {
      return await GetLista(`${SistemaApisRotas.Lista.Usuarios}`);
    },

    async GruposUsuarios() {
      return await GetLista(`${SistemaApisRotas.Lista.GruposUsuarios}`);
    },

    async GruposMenus() {
      return await GetLista(`${SistemaApisRotas.Lista.GruposMenus}`);
    },

    async Menus() {
      return await GetLista(`${SistemaApisRotas.Lista.Menus}`);
    },

  },

  // Rotas de Api para Parâmetros
  Parametro :{
    async Edit(id) {
      return await Edit(`${SistemaApisRotas.Parametro.Edit}`, `${id}`);
    },

    async Save(dados) {
      return await Save(`${SistemaApisRotas.Parametro.Save}`, dados);
    },
  },

  // Rotaa de Api para Arquivo
  Arquivo: {
    async Upload(dados) {
      return await FileUpload(`${SistemaApisRotas.Arquivo.Upload}`, dados);
    },

    async Download(dados) {
      return await Save(`${SistemaApisRotas.Arquivo.Download}`, dados);
    },
  }
  
};

export default SistemaApis;
