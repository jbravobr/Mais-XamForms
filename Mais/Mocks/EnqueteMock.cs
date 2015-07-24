using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mais
{
	public static class EnqueteMock
	{
		public static List<Enquete> MockEnquetes()
		{
			try
			{				
				return new List<Enquete>
				{
					#region -- Públicas --
					new Enquete
					{ 
						Titulo = "Enquete 1",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 1",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa A"  },
								new Resposta{ TextoResposta = "Alternativa B"  },
								new Resposta{ TextoResposta = "Alternativa C"  },
								new Resposta{ TextoResposta = "Alternativa D"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Saúde"
						},
						Imagem = "banner_1.jpeg",
						UrlImagem = "http://www.google.com.br",
						UrlVideo = "https://www.youtube.com/embed/C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Publica
					},
					new Enquete
					{ 
						Titulo = "Enquete 2",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 2",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa AA"  },
								new Resposta{ TextoResposta = "Alternativa BB"  },
								new Resposta{ TextoResposta = "Alternativa CC"  },
								new Resposta{ TextoResposta = "Alternativa DD"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Saúde"
						},
						Imagem = "banner_2.jpg",
						//UrlImagem = "http://www.google.com.br",
						UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Publica
					},
					new Enquete
					{ 
						Titulo = "Enquete 3",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 3",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa AAA"  },
								new Resposta{ TextoResposta = "Alternativa BBB"  },
								new Resposta{ TextoResposta = "Alternativa CCC"  },
								new Resposta{ TextoResposta = "Alternativa DDD"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Tecnologia"
						},
						Imagem = "banner_3.jpg",
						//UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Publica
					},
					new Enquete
					{ 
						Titulo = "Enquete 4",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 4",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa AB"  },
								new Resposta{ TextoResposta = "Alternativa BC"  },
								new Resposta{ TextoResposta = "Alternativa CD"  },
								new Resposta{ TextoResposta = "Alternativa DE"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Tecnologia"
						},
						Imagem = "banner_4.jpg",
						//UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Publica
					},
					new Enquete
					{ 
						Titulo = "Enquete 5",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 1",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa ABB"  },
								new Resposta{ TextoResposta = "Alternativa BCC"  },
								new Resposta{ TextoResposta = "Alternativa CDD"  },
								new Resposta{ TextoResposta = "Alternativa DEE"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Estilo"
						},
						Imagem = "banner_5.jpg",
						//UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Publica
					},
					new Enquete
					{ 
						Titulo = "Enquete 6",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 6",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa X"  },
								new Resposta{ TextoResposta = "Alternativa Y"  },
								new Resposta{ TextoResposta = "Alternativa Z"  },
								new Resposta{ TextoResposta = "Alternativa W"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Estilo"
						},
						Imagem = "banner_6.jpg",
						//UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Publica
					},
					new Enquete
					{ 
						Titulo = "Enquete 7",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 7",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa XX"  },
								new Resposta{ TextoResposta = "Alternativa YY"  },
								new Resposta{ TextoResposta = "Alternativa ZZ"  },
								new Resposta{ TextoResposta = "Alternativa WW"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Ciência"
						},
						Imagem = "banner_7.jpeg",
						//UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Publica
					},
					new Enquete
					{ 
						Titulo = "Enquete 8",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 8",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa XX"  },
								new Resposta{ TextoResposta = "Alternativa YY"  },
								new Resposta{ TextoResposta = "Alternativa ZZ"  },
								new Resposta{ TextoResposta = "Alternativa WW"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Ciência"
						},
						Imagem = "banner_7.jpeg",
						//UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Publica
					},
					new Enquete
					{ 
						Titulo = "Enquete 9",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 9",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa XX"  },
								new Resposta{ TextoResposta = "Alternativa YY"  },
								new Resposta{ TextoResposta = "Alternativa ZZ"  },
								new Resposta{ TextoResposta = "Alternativa WW"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Política"
						},
						Imagem = "banner_7.jpeg",
						//UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Publica
					},
					new Enquete
					{ 
						Titulo = "Enquete 10",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 10",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa XX"  },
								new Resposta{ TextoResposta = "Alternativa YY"  },
								new Resposta{ TextoResposta = "Alternativa ZZ"  },
								new Resposta{ TextoResposta = "Alternativa WW"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Entreterimento"
						},
						Imagem = "banner_7.jpeg",
						//UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Publica
					},
					#endregion

					#region -- Interesse --

					new Enquete
					{  
						Titulo = "Enquete 1",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 1",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa A"  },
								new Resposta{ TextoResposta = "Alternativa B"  },
								new Resposta{ TextoResposta = "Alternativa C"  },
								new Resposta{ TextoResposta = "Alternativa D"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Esporte"
						},
						Imagem = "banner_1.jpeg",
						//UrlImagem = "http://www.google.com.br",
						UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Interesse
					},
					new Enquete
					{  
						Titulo = "Enquete 2",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 2",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa AA"  },
								new Resposta{ TextoResposta = "Alternativa BB"  },
								new Resposta{ TextoResposta = "Alternativa CC"  },
								new Resposta{ TextoResposta = "Alternativa DD"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Esporte"
						},
						Imagem = "banner_2.jpg",
						UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Interesse
					},
					new Enquete
					{  
						Titulo = "Enquete 3",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 3",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa AAA"  },
								new Resposta{ TextoResposta = "Alternativa BBB"  },
								new Resposta{ TextoResposta = "Alternativa CCC"  },
								new Resposta{ TextoResposta = "Alternativa DDD"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Esporte"
						},
						Imagem = "banner_3.jpg",
						UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Interesse
					},
					new Enquete
					{  
						Titulo = "Enquete 4",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 4",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa AB"  },
								new Resposta{ TextoResposta = "Alternativa BC"  },
								new Resposta{ TextoResposta = "Alternativa CD"  },
								new Resposta{ TextoResposta = "Alternativa DE"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Esporte"
						},
						Imagem = "banner_4.jpg",
						UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Interesse
					},
					new Enquete
					{  
						Titulo = "Enquete 5",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 1",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa ABB"  },
								new Resposta{ TextoResposta = "Alternativa BCC"  },
								new Resposta{ TextoResposta = "Alternativa CDD"  },
								new Resposta{ TextoResposta = "Alternativa DEE"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Esporte"
						},
						Imagem = "banner_5.jpg",
						UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Interesse
					},
					new Enquete
					{  
						Titulo = "Enquete 6",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 6",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa X"  },
								new Resposta{ TextoResposta = "Alternativa Y"  },
								new Resposta{ TextoResposta = "Alternativa Z"  },
								new Resposta{ TextoResposta = "Alternativa W"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Esporte"
						},
						Imagem = "banner_6.jpg",
						UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Interesse
					},
					new Enquete
					{  
						Titulo = "Enquete 7",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 7",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa XX"  },
								new Resposta{ TextoResposta = "Alternativa YY"  },
								new Resposta{ TextoResposta = "Alternativa ZZ"  },
								new Resposta{ TextoResposta = "Alternativa WW"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Esporte"
						},
						Imagem = "banner_7.jpeg",
						UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Interesse
					},
					new Enquete
					{  
						Titulo = "Enquete 8",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 8",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa XX"  },
								new Resposta{ TextoResposta = "Alternativa YY"  },
								new Resposta{ TextoResposta = "Alternativa ZZ"  },
								new Resposta{ TextoResposta = "Alternativa WW"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Esporte"
						},
						Imagem = "banner_7.jpeg",
						UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Interesse
					},
					new Enquete
					{  
						Titulo = "Enquete 9",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 9",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa XX"  },
								new Resposta{ TextoResposta = "Alternativa YY"  },
								new Resposta{ TextoResposta = "Alternativa ZZ"  },
								new Resposta{ TextoResposta = "Alternativa WW"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Esporte"
						},
						Imagem = "banner_7.jpeg",
						UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Interesse
					},
					new Enquete
					{  
						Titulo = "Enquete 10",
						Pergunta = new Pergunta
						{
							TextoPergunta = "Pergunta para a Enquete 10",
							Respostas = new List<Resposta>
							{
								new Resposta{ TextoResposta = "Alternativa XX"  },
								new Resposta{ TextoResposta = "Alternativa YY"  },
								new Resposta{ TextoResposta = "Alternativa ZZ"  },
								new Resposta{ TextoResposta = "Alternativa WW"  }
							}
						},
						Categoria = 
							new Categoria
						{
							Nome = "Esporte"
						},
						Imagem = "banner_7.jpeg",
						UrlImagem = "http://www.google.com.br",
						//UrlVideo = "https://www.youtube.com/watch?v=C0DPdy98e4c",
						EnqueteRespondida = false,
						Tipo = EnumTipoEnquete.Interesse
					}


					#endregion
				};
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}

