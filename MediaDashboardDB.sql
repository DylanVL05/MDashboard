USE [master]
GO
/****** Object:  Database [MediaDashboard]    Script Date: 23/4/2025 12:45:15 ******/
CREATE DATABASE [MediaDashboard]
GO
ALTER DATABASE [MediaDashboard] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MediaDashboard] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MediaDashboard] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MediaDashboard] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MediaDashboard] SET ARITHABORT OFF 
GO
ALTER DATABASE [MediaDashboard] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MediaDashboard] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MediaDashboard] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MediaDashboard] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MediaDashboard] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MediaDashboard] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MediaDashboard] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MediaDashboard] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MediaDashboard] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MediaDashboard] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MediaDashboard] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MediaDashboard] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MediaDashboard] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MediaDashboard] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MediaDashboard] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MediaDashboard] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MediaDashboard] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MediaDashboard] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MediaDashboard] SET  MULTI_USER 
GO
ALTER DATABASE [MediaDashboard] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MediaDashboard] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MediaDashboard] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MediaDashboard] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MediaDashboard] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MediaDashboard] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [MediaDashboard] SET QUERY_STORE = ON
GO
ALTER DATABASE [MediaDashboard] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MediaDashboard]
GO
/****** Object:  Table [dbo].[Components]    Script Date: 23/4/2025 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Components](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Tipo] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConfiguracionWidgets]    Script Date: 23/4/2025 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConfiguracionWidgets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[WidgetId] [int] NOT NULL,
	[Height] [int] NULL,
	[Width] [int] NULL,
	[EsFavorito] [bit] NULL,
	[EsVisible] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 23/4/2025 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[Accion] [nvarchar](255) NOT NULL,
	[Fecha] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermisosWidgets]    Script Date: 23/4/2025 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermisosWidgets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[WidgetId] [int] NOT NULL,
	[PuedeVer] [bit] NULL,
	[PuedeEditar] [bit] NULL,
	[PuedeEliminar] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 23/4/2025 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Rol] [nvarchar](50) NULL,
	[FechaRegistro] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Widgets]    Script Date: 23/4/2025 12:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Widgets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](255) NULL,
	[ComponentId] [int] NOT NULL,
	[URL_API] [nvarchar](500) NOT NULL,
	[API_Key] [nvarchar](255) NULL,
	[EsPublico] [bit] NULL,
	[Estado] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Components] ON 
GO
INSERT [dbo].[Components] ([Id], [Tipo], [Descripcion]) VALUES (1, N'card', N'Tarjeta para resaltar información clave')
GO
INSERT [dbo].[Components] ([Id], [Tipo], [Descripcion]) VALUES (2, N'chart', N'Gráfica para mostrar tendencias y análisis')
GO
INSERT [dbo].[Components] ([Id], [Tipo], [Descripcion]) VALUES (3, N'table', N'Visualización de datos en formato tabular')
GO
SET IDENTITY_INSERT [dbo].[Components] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 
GO
INSERT [dbo].[Usuarios] ([Id], [Nombre], [Email], [PasswordHash], [Rol], [FechaRegistro]) VALUES (2, N'Alejo', N'Ale@gmail.com', N'123', N'User', NULL)
GO
INSERT [dbo].[Usuarios] ([Id], [Nombre], [Email], [PasswordHash], [Rol], [FechaRegistro]) VALUES (3, N'Prueba', N'Prueba@gmail.com', N'123', N'User', CAST(N'2025-04-16T18:02:52.030' AS DateTime))
GO
INSERT [dbo].[Usuarios] ([Id], [Nombre], [Email], [PasswordHash], [Rol], [FechaRegistro]) VALUES (5, N'Jose', N'Jose@gmail.com', N'123', N'Admin', CAST(N'2025-04-23T01:34:14.773' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
SET IDENTITY_INSERT [dbo].[Widgets] ON 
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (1, N'NASA APOD', N'Widget para mostrar la imagen del día de la NASA', 1, N'https://api.nasa.gov/planetary/apod', N'fnq5xNCvymacRJ4bevQqhFCNbjPX3epMx1wGO6MJ', 1, 1)
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (2, N'ExchangeRate', N'TipodeCambio', 2, N'https://v6.exchangerate-api.com/', N'3623374e0db81dd8a4336650', 1, 1)
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (3, N'Rick API', N'Un API de Rick and Morty', 1, N'https://rickandmortyapi.com/api/character/?name=rick&status=alive', N'null', 1, 1)
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (4, N'JokeApi', N'Api que cuenta chistes.', 2, N'https://v2.jokeapi.dev/joke/Any?lang=es', N'null', 1, 1)
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (5, N'MeowFacts API', N'Api que cuenta datos curiosos sobre los gatos', 1, N'https://meowfacts.herokuapp.com/', N'null', 1, 1)
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (6, N'Clima API', N'Clima', 2, N'https://api.openweathermap.org/data/2.5/weather', N'eb2a4e2dcf7216af5facb82570a87c29', 1, 1)
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (7, N'Dog API', N'API para visualizar imagenes de perros', 1, N'https://dog.ceo/api/breeds/image/random', N'null', 1, 1)
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (8, N'Logotype API', N'API para información sobre logotipos', 1, N'https://www.logotypes.dev/random/data', N'null', 1, 1)
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (9, N'Chuck Norris API', N'API para obtener frases aleatorias de Chuck Norris', 1, N'https://api.chucknorris.io/jokes/random', N'NULL', 1, 1)
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (10, N'Quote Api', N'Api para frases celebres.', 1, N'https://thequotesapi.onrender.com/quotes/random', N'live_ekvdK5iOssh0hBPF2y0yoKwIsDrhTZfaofCkdJJxx6QcCe9XBVPRxdYYmVPSdvl9', 1, 1)
GO
INSERT [dbo].[Widgets] ([Id], [Nombre], [Descripcion], [ComponentId], [URL_API], [API_Key], [EsPublico], [Estado]) VALUES (11, N'Advice', N'Api para frases.', 2, N'https://api.adviceslip.com/advice', N'null', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Widgets] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Usuarios__A9D10534EDC4BE82]    Script Date: 23/4/2025 12:45:15 ******/
ALTER TABLE [dbo].[Usuarios] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConfiguracionWidgets] ADD  DEFAULT ((0)) FOR [EsFavorito]
GO
ALTER TABLE [dbo].[ConfiguracionWidgets] ADD  DEFAULT ((1)) FOR [EsVisible]
GO
ALTER TABLE [dbo].[Logs] ADD  DEFAULT (getdate()) FOR [Fecha]
GO
ALTER TABLE [dbo].[PermisosWidgets] ADD  DEFAULT ((1)) FOR [PuedeVer]
GO
ALTER TABLE [dbo].[PermisosWidgets] ADD  DEFAULT ((0)) FOR [PuedeEditar]
GO
ALTER TABLE [dbo].[PermisosWidgets] ADD  DEFAULT ((0)) FOR [PuedeEliminar]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ('Usuario') FOR [Rol]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Widgets] ADD  DEFAULT ((1)) FOR [EsPublico]
GO
ALTER TABLE [dbo].[Widgets] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[ConfiguracionWidgets]  WITH CHECK ADD FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ConfiguracionWidgets]  WITH CHECK ADD FOREIGN KEY([WidgetId])
REFERENCES [dbo].[Widgets] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Logs]  WITH CHECK ADD FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PermisosWidgets]  WITH CHECK ADD FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PermisosWidgets]  WITH CHECK ADD FOREIGN KEY([WidgetId])
REFERENCES [dbo].[Widgets] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Widgets]  WITH CHECK ADD FOREIGN KEY([ComponentId])
REFERENCES [dbo].[Components] ([Id])
ON DELETE CASCADE
GO
USE [master]
GO
ALTER DATABASE [MediaDashboard] SET  READ_WRITE 
GO