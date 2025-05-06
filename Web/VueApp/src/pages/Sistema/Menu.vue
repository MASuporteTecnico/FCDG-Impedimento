<template>
  <v-container>
    <v-form :disabled="Permissao.SomenteLeitura">
      <v-row>
        <v-col cols="6" lg="5" md="5" offset-lg="0" >
          <v-treeview open-all :disabled="Permissao.SomenteLeitura" :items="[Model]" v-model="MenuSelecionado" v-model:activated="MenuAtivado" return-object mandatory activatable item-children="SubMenu" select-strategy="single-leaf" :expand-icon="null" :collapse-icon="null" @update:activated="ItemChanged($event)">
            <template v-slot:prepend="{ item, isOpen }">
              <v-icon>
                {{ item.Icone }}
              </v-icon>
            </template>
          </v-treeview>
        </v-col>
        <v-col cols="6" lg="5">
          <v-row>
            <v-col>
              <v-row>
                <v-col>
                  <br />
                  <v-btn :disabled="!HabilitadoCriarNovo" @click="NovoItemMenu(Item)" v-if="!Permissao.SomenteLeitura"> Adicionar Novo Item de Menu<v-icon> mdi-plus </v-icon> </v-btn>
                </v-col>
              </v-row>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <v-form :disabled="!ItemSelecionado">
                <v-row>
                  <v-col
                    ><br />
                    <div class="text-subtitle-1 text-medium-emphasis">Alterar Posição do Menu</div>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col> <v-text-field v-model="Item.Nome" label="Nome"></v-text-field> </v-col>
                </v-row>
                <v-row>
                  <v-col> <v-text-field v-model="Item.Rota" label="Rota"></v-text-field> </v-col>
                </v-row>
                <v-row>
                  <v-col> <v-text-field v-model="Item.Icone" label="Icone"></v-text-field> </v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-text-field v-model="Item.Ordem" label="Ordem" readonly>
                      <template v-slot:append-inner>
                        <v-btn variant="tonal" rounded="0" @click="OrderChangedDown()" v-if="!Permissao.SomenteLeitura">
                          <v-icon size="x-large" class="ma-0 pa-0"> mdi-chevron-down</v-icon>
                        </v-btn>
                        <v-btn variant="tonal" rounded="0" @click="OrderChangedUp()" v-if="!Permissao.SomenteLeitura">
                          <v-icon size="x-large" class="ma-0 pa-0"> mdi-chevron-up </v-icon>
                        </v-btn>
                      </template>
                    </v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col> <v-switch v-model="Item.Divisor" label="Divisor" @change="MudaDivisor($event)"></v-switch></v-col>
                </v-row>
                <v-row>
                  <v-col>
                    <v-btn block color="warning" @click="ExcluirItemMenu(Item)" :disabled="!ItemSelecionado" v-if="!Permissao.SomenteLeitura"> Excluir Item de Menu<v-icon> mdi-times </v-icon> </v-btn>
                  </v-col>
                </v-row>
              </v-form>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-form>
    <SaveDelCancel NoDelete :NoChanges="NoChanges" :ReadOnly="Permissao.SomenteLeitura"  v-on:save="Save()" v-on:cancel="Edit()"></SaveDelCancel>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Configuração do Menu",
  },
});

import { inject } from "vue";
import { useAppStore } from "@/stores/app";

const api = inject("SistemaApis");
let Model = ref({});
let ModelOriginal = ref({});
let Item = ref({});
let ItemRoot = ref(false);
let MenuSelecionado = ref([]);
let MenuAtivado = ref([])

const store = useAppStore();

const Permissao = computed(() => {
  return store.GetPermissao;
});

const NoChanges = computed(() => {
  return (JSON.stringify(Model.value) === JSON.stringify(ModelOriginal.value));
});

const ItemSelecionado = computed(() => {
  return JSON.stringify(Item.value) === JSON.stringify({}) ? false : true;
});

const HabilitadoCriarNovo = computed(() => {
  if (ItemRoot.value) return true;
  else {
    if (!(JSON.stringify(Item.value) === JSON.stringify({}))) {
      return Item.value.MenuPaiId <= 1 ? true : false;
    }
  }

  return false;
});

async function Edit() {
  Model.value = [];
  Item.value = {};
  let response = await api.Menu.Edit(1);
  Model.value = response.Dados;
  ModelOriginal.value = response.Dados;
}

async function Save() {
  try {
    Item.value = {};
    let response = await api.Menu.Save(Model.value);
    Model.value = response.Dados;
  } catch {}
}

function ItemChanged(item) {

  if (item.length > 0) {
    if (item[0].Id == 1) {
      Item.value = {};
      ItemRoot.value = true;
      return;
    }
    Item.value = item[0];
  } else {
    Item.value = {};
  }
  ItemRoot.value = false;
}

function NovoItemMenu(item) {
  let total = 0;
  let novoIcone = "";
  let novaRota = "";
  let menuPaiId = 0;

  if (ItemRoot.value) {
    total = Model.value.SubMenu.length;
    menuPaiId = Model.value.Id;
  } else {
    total = item.SubMenu.length;
    novoIcone = item.MenuPaiId <= 1 ? "mdi-chevron-right" : "";
    novaRota = item.MenuPaiId == 1 && total > 0 ? "/#" : "";
    menuPaiId = item.Id;
  }

  let novoItemMenu = {
    Nome: "Novo Item Menu",
    Icone: `${novoIcone}`,
    MenuPaiId: menuPaiId,
    Ordem: total+1,
    Rota: `${novaRota}`,
    Dividor: false,
    Ativo: true,
    SubMenu: []
  };

  if (ItemRoot.value) {
    Model.value.SubMenu.push(novoItemMenu);
  } else {
    item.SubMenu.push(novoItemMenu);
  }

  MenuAtivado.value[0] = novoItemMenu;
  Item.value = novoItemMenu;
  ItemChanged(MenuAtivado.value);
}

function MudaDivisor(data)
{
  MenuAtivado.value[0].Icone = "mdi-ray-start-end";  
  MenuAtivado.value[0].Nome = "<divisor>";
  MenuAtivado.value[0].Rota = "";
}

function ExcluirItemMenu(item) {
  let MenuPai = {};

  if (ItemRoot.value) {
    return;
  } else {
    if (item.MenuPaiId > 1) {
      MenuPai = Model.value.SubMenu.find(({ Id }) => Id === item.MenuPaiId);
    } else {
      MenuPai = Model.value;
    }
  }

  MenuPai.SubMenu.splice(MenuPai.SubMenu.indexOf(item), 1);
}

function OrderChangedUp() {
  if (JSON.stringify(Item.value) === JSON.stringify({})) return;

  let MenuPai = {};

  if (Item.value.Ordem == 1) return;

  if (Item.value.MenuPaiId > 1) {
    MenuPai = Model.value.SubMenu.find(({ Id }) => Id === Item.value.MenuPaiId);
  } else {
    MenuPai = Model.value;
  }

  let ItemAnterior = MenuPai.SubMenu[MenuPai.SubMenu.indexOf(Item.value) - 1];

  Item.value.Ordem--;
  ItemAnterior.Ordem++;
  MenuPai.SubMenu = MenuPai.SubMenu.sort((a, b) => a.Ordem - b.Ordem);
}

function OrderChangedDown() {
  if (JSON.stringify(Item.value) === JSON.stringify({})) return;

  let MenuPai = {};
  let total = 0;

  if (Item.value.MenuPaiId > 1) {
    MenuPai = Model.value.SubMenu.find(({ Id }) => Id === Item.value.MenuPaiId);
    total = MenuPai.SubMenu.length;
  } else {
    MenuPai = Model.value;
    total = MenuPai.SubMenu.length;
  }

  if (Item.value.Ordem == total) return;

  let ItemPosterior = MenuPai.SubMenu[MenuPai.SubMenu.indexOf(Item.value) + 1];
  Item.value.Ordem++;
  ItemPosterior.Ordem--;
  MenuPai.SubMenu = MenuPai.SubMenu.sort((a, b) => a.Ordem - b.Ordem);
}

onMounted(async () => {
  await Edit();
});
</script>
