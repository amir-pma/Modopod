<template>
  <div class="app">
    <transition name="fade" mode="out-in">
      <router-view />
    </transition>
  </div>
</template>

<script>
    export default {
        name: "app"
    };
</script>

<style>
    html,
    body {
        height: 100vh;

    }

    div.app,
    div.page {
        height: 100% !important;
    }

    .fade-enter-active,
    .fade-leave-active {
        transition: opacity 0.3s ease-in-out;
    }

    .fade-enter,
    .fade-leave-to {
        opacity: 0;
    }
</style>
