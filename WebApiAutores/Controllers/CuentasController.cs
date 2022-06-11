using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        //se injecta el servicio UserManager  que permite registrar un usuario  con la clase IdentityUser UserManager<IdentityUser>
        //se injecta el servicio de SignInManager para poder hacer login de Usuario
        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }



        //Se genera el EndPoint de Registrar al usuario donde se crea al usuario y en caso de que se genere correctamente
        //se construye el Token por medio de una funcion la cual regresa un objeto RespuestaAutenticacion con el Token y la expiracion
        //en caso de que no se registre correctamente el usuario entonces retorna un BadRequest con el error.
        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
        {

            var usuario = new IdentityUser
            {
                UserName =  credencialesUsuario.Email,
                Email =  credencialesUsuario.Email
            };

            var resultado = await userManager.CreateAsync(usuario, credencialesUsuario.Password);

            if(resultado.Succeeded)
            {
                return ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }



        }

        //Se crea el EndPoint de Login donde tambien regresa el Token y la expiracion que devuelve la funcion ConstruirToken
        //con PasswordSignInAsync se intenta loguear al usuario que se le pasa como argumento

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password, 
                isPersistent: false, lockoutOnFailure: false);

            if(resultado.Succeeded)
            {
                return ConstruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }



        }


        //Se realiza la Funcion que genera  el Token, en esta funcion se retorna un objeto RespuestaAutenticacion (clase que tiene Token y Expiracion)
        //y se introduce un objeto Credenciales (clase que solo tiene las propiedades Email y Password )  ya que se necesita la informacion del usuario para 
        //generar los Claims
        private RespuestaAutenticacion ConstruirToken(CredencialesUsuario credencialesUsuario)
        {

            //se genera la lista de Claims donde por esta ocasion solo se pondra el Email pero se puede poner lo que desees

            List<Claim> claims = new List<Claim>() 
            {
                new Claim("email", credencialesUsuario.Email)
            };

             //se obtiene la llave desde el provedor de configuracion injectando el Iconfiguration
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));

            //se define el algoritmo  HmacSha256
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            //se define la expiracion

            var expiracion = DateTime.UtcNow.AddDays(1);

            //se contruye el Token

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            
            //se crea un objeto RespuestaAutenticacion y se retorma
            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };







        }

        

        




    }
}
