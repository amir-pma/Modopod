<template>
    <div class="d-flex align-items-center justify-content-center text-right h-100" v-model="index">
        <login-form class="p-4" style="border: 1px solid gray ; border-radius: 5px;"/>
    </div>
</template>

<script>
    import LoginForm from "./LoginForm.vue";

    export default {
        name: "login",
        components: {
            LoginForm,
        },
        data() {
            return {
                index: 0,
                registered: false
            };
        },
        methods: {
            success() {
                this.registered = true;
                this.index = 0;
            },
            close() {
                this.$store.commit("hideAuthModal");
                let query = Object.assign({}, this.$route.query);
                delete query.redirect;
                this.$router.push({ query: query });
            }
        }
    };
</script>

<style>
    .modal-backdrop {
        opacity: 0.5 !important;
    }
</style>