<template>
    <transition name="fade">
      <div class="modal-mask">
        <v-container fluid class="fill-height" >
          <v-row align="center" justify="center" class="fill-height" >
            <v-col>
                <v-progress-circular indeterminate :color="color" size="96" width="6"> </v-progress-circular>
            </v-col>
          </v-row>
        </v-container>
      </div>
    </transition>
</template>

<script>
export default {
  name: "loading",
  data() {
    return {
      item: 0,
    };
  },
  methods: {
    getColor() {
      if (this.item == 7) {
        this.item = 0;
        return this.item;
      }

      this.item = this.item + 1;
      return this.item;
    },
  },
  computed: {
    color() {
      return ["#4285F4", "orange", "#DE3E35", "teal", "#F7C223", "yellow", "#1B9A59", "indigo"][this.item];
    },
  },
  created: function () {
    this.timer = setInterval(this.getColor, 400);
  },
  beforeDestroy() {
    clearInterval(this.timer);
  },
};
</script>

<style scoped>
.modal-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.2);
  display: table;
  transition: opacity 0.3s ease;
  text-align: center;
}

.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.modal-container {
  width: 300px;
  margin: 0px auto;
  padding: 20px 30px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  transition: all 0.3s ease;
  font-family: Helvetica, Arial, sans-serif;
}

.modal-header h3 {
  margin-top: 0;
  color: #42b983;
}

.modal-body {
  margin: 20px 0;
}

.modal-default-button {
  float: right;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s;
}

.fade-enter, .fade-leave-to /* .fade-leave-active em versões anteriores a 2.1.8 */ {
  opacity: 0;
}
</style>
