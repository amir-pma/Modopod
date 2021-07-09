<template>
  <form @submit.prevent="login" class="p-2" >
    <b-alert variant="danger" :show="error !== null" dismissible @dismissed="error = null">
      {{ error }}
    </b-alert>
    <b-alert variant="success" :show="registered && error === null">
      ثبت نام موفق. برای ادامه وارد شوید
    </b-alert>
    <p>با نام کاربری یا ایمیل و پسورد وارد شوید</p>
    <b-form-group label="ایمیل / نام کاربری">
      <b-form-input v-model.trim="username" />
    </b-form-group>
    <b-form-group label="پسورد">
      <b-form-input v-model.trim="password" type="password" />
    </b-form-group>
    <b-form-group>
      <b-button variant="primary" type="submit" :disabled="loading">ورود</b-button>
      <b-button variant="default" @click="close" :disabled="loading">لغو</b-button>
    </b-form-group>
  </form>
</template>

<script>
    export default {
        name: "login-form",
        props: {
            registered: {
                type: Boolean,
                required: false
            }
        },
        data() {
            return {
                username: "",
                password: "",
                error: null
            };
        },
        computed: {
            loading() {
                return this.$store.state.loading;
            }
        },
        mounted: () => {
            var accessToken = localStorage.getItem('accessToken');
            var wpToken = localStorage.getItem('wpToken');
            if (accessToken && wpToken) {
                location.replace('/makeproduct')
            }
        },
        methods: {
            login() {
                const payload = {
                    username: this.username,
                    password: this.password
                };

                this.$store
                    .dispatch("login", payload)
                    .then(response => {
                        console.log(response);
                        localStorage.setItem('accessToken', response.data.access_token)
                        localStorage.setItem('wpToken', response.data.wpToken)
                        this.error = null;
                        this.username = "";
                        this.password = "";

                        this.$router.push("makeproduct");
                        //if (this.$route.query.redirect) {
                        //  this.$router.push(this.$route.query.redirect);
                        //}
                    })
                    .catch(error => {
                        this.error = error.data;
                    });
            },
            close() {
                this.$emit("close");
            },
            loginToHome() {
                this.$router.push("makeproduct");

            }
        }
    };
</script>

