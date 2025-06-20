<style scoped>
.blinking-class {
  animation: blink-animation 1s infinite;
}

@keyframes blink-animation {
  0%,
  49%,
  100% {
    opacity: 1;
  }
  50%,
  99% {
    opacity: 0;
  }
}
</style>

<template>
  <span title="Tempo de sessão">
    <v-btn stacked :class="isBlinking ? 'blinking-class' : ' '">
      <v-badge :color="isBlinking ? 'error' : 'info'">
        <template v-slot:badge>
          {{ display }}
        </template>
        <v-icon >mdi-clock-outline</v-icon>
      </v-badge>
    </v-btn>
  </span>
  <Confirm Msg="Você será desconectado." ConfirmTxt="Continuar conectado." NoCancel :Show="dialog" @confirm="dialog = !dialog"></Confirm>
  
</template>

<script>
export default {
  name: "AutoLogoff",
  emits: ["idle", "remind"],
  data: function () {
    return {
      display: "",
      timer: undefined,
      start: 0,
      counter: undefined,
      diff: 0,
      minutes: "",
      seconds: "",
      isBlinking: false,
      dialog: false
    };
  },
  props: {
    duration: {
      type: Number,
      // default 5 minutes
      default: 60 * 5,
    },
    events: {
      type: Array,
      default: () => ["mousemove", "click", "keypress"],
    },
    loop: {
      type: Boolean,
      default: false,
    },
    reminders: {
      type: Array,
      // array of seconds
      // emit "remind" event on each second
      default: () => [],
    },
    wait: {
      type: Number,
      default: 0,
    },
  },
  mounted() {
    setTimeout(() => {
      this.start = Date.now();
      this.setDisplay();
      this.$nextTick(() => {
        this.setTimer();
        for (let i = this.events.length - 1; i >= 0; i -= 1) {
          window.addEventListener(this.events[i], this.clearTimer);
        }
      });
    }, this.wait * 1000);
  },
  methods: {
    startBlinking() {
      this.isBlinking = true;
    },
    stopBlinking() {
      this.isBlinking = false;
    },
    setDisplay() {
      // seconds since start
      this.diff = this.duration - (((Date.now() - this.start) / 1000) | 0);
      //console.log(this.diff);
      if (this.diff < 0 && !this.loop) {
        return;
      }
      this.shouldRemind();
      // bitwise OR to handle parseInt
      const minute = (this.diff / 60) | 0;
      const second = this.diff % 60 | 0;
      this.minutes = `${minute < 10 ? "0" + minute : minute}`;
      this.seconds = `${second < 10 ? "0" + second : second}`;
      this.display = `${this.minutes}:${this.seconds}`;
    },
    shouldRemind() {
      if (this.reminders.length > 0) {
        if (this.reminders.includes(this.diff)) {
          this.remind();
        }
      }
    },
    countdown() {
      this.setDisplay();
      if (this.diff <= 0 && this.loop) {
        // add second to start at the full duration
        // for instance 05:00, not 04:59
        this.start = Date.now() + 1000;
      }
    },
    idle() {
      this.$emit("idle");
    },
    remind() {
      this.startBlinking();
      this.dialog = true;
      this.$emit("remind");
    },
    setTimer() {
      this.timer = window.setInterval(this.idle, this.duration * 1000);
      this.counter = window.setInterval(this.countdown, 1000);
    },
    clearTimer() {
      this.stopBlinking();
      clearInterval(this.timer);
      clearInterval(this.counter);
      this.setDisplay();
      this.start = Date.now();
      this.diff = 0;
      this.setTimer();
    },
  },
  beforeDestroy() {
    clearInterval(this.timer);
    clearInterval(this.counter);
    for (let i = this.events.length - 1; i >= 0; i -= 1) {
      window.removeEventListener(this.events[i], this.clearTimer);
    }
  },
};
</script>
