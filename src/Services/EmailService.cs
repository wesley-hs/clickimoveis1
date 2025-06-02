// Services/EmailService.cs
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace click_imoveis.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task EnviarEmailAsync(string destinatario, string assunto, string mensagemHtml)
        {
            using var smtpClient = new SmtpClient();
            var mensagem = new MimeMessage();

            try
            {
                // Configuração da mensagem
                mensagem.From.Add(new MailboxAddress(
                    _config["EmailSettings:NomeRemetente"],
                    _config["EmailSettings:EmailRemetente"]
                ));
                mensagem.To.Add(MailboxAddress.Parse(destinatario));
                mensagem.Subject = assunto;
                mensagem.Body = new TextPart("html") { Text = mensagemHtml };

                // Configuração SMTP com timeout
                var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(15)).Token;

                await smtpClient.ConnectAsync(
                    _config["EmailSettings:ServidorSmtp"],
                    int.Parse(_config["EmailSettings:Porta"]),
                    SecureSocketOptions.StartTlsWhenAvailable,
                    timeout
                );

                await smtpClient.AuthenticateAsync(
                    _config["EmailSettings:EmailRemetente"],
                    _config["EmailSettings:Senha"],
                    timeout
                );

                // Envio do e-mail
                await smtpClient.SendAsync(mensagem, timeout);
                _logger.LogInformation($"E-mail enviado com sucesso para: {destinatario}");
            }
            catch (AuthenticationException ex)
            {
                _logger.LogError(ex, "Falha na autenticação SMTP. Verifique usuário e senha.");
                throw new Exception("Falha na autenticação do servidor de e-mail.", ex);
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError(ex, $"Erro SMTP (Status: {ex.StatusCode}). Falha ao enviar para {destinatario}");
                throw new Exception("Erro durante o envio do e-mail.", ex);
            }
            catch (SmtpProtocolException ex)
            {
                _logger.LogError(ex, $"Erro de protocolo SMTP ao enviar para {destinatario}");
                throw new Exception("Erro na comunicação com o servidor de e-mail.", ex);
            }
            catch (OperationCanceledException)
            {
                _logger.LogError("Timeout ao tentar enviar e-mail");
                throw new Exception("Tempo limite excedido na operação.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado ao enviar e-mail para {destinatario}");
                throw new Exception("Ocorreu um erro inesperado.", ex);
            }
            finally
            {
                if (smtpClient.IsConnected)
                {
                    await smtpClient.DisconnectAsync(true);
                }

                [AllowAnonymous]
                [HttpGet("testar-smtp")]
                 async Task<string> TestarSMTP([FromServices] EmailService emailService)
                {
                    try
                    {
                        await emailService.EnviarEmailAsync(
                            "hwesley478@gmail.com",
                            "Teste SMTP",
                            "<h1>Funcionou!</h1><p>Se recebeu este e-mail, o serviço está configurado corretamente.</p>"
                        );
                        return "E-mail enviado com sucesso! Verifique sua caixa de entrada.";
                    }
                    catch (Exception ex)
                    {
                        return $"ERRO: {ex.Message}";
                    }
                }
            }
        }
    }
}