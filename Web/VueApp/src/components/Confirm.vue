<template>
  <v-dialog persistent v-model="showDialog" @keydown.enter="returnTrue()" @keydown.esc="returnFalse()" width="420px">
    <v-card>
      <v-toolbar id="tollbar_top" color="primary" :title="Msg"></v-toolbar>

      <v-card-text class="text-h2 pa-12">
        <v-row>
          <v-col v-if="!NoCancel" >
            <v-btn color="error" @click="returnFalse" block>Não</v-btn>
          </v-col>
          <v-col v-if="!NoCancel"> </v-col>
          <v-col>
            <v-btn color="success" @click="returnTrue" block> {{ ConfirmTxt || 'Sim'}}</v-btn>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: "confirm",
  props: {
    Msg: String,
    ConfirmTxt: String,
    Show: Boolean,
    NoCancel: { type: Boolean, default: false },
  },
  methods: {
    returnFalse() {
      this.$emit("cancel");
    },
    returnTrue() {
      this.$emit("confirm");
    },
  },
  computed: {
    showDialog() {
      return this.Show;
    },
  },
};
</script>
