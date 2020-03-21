/****** Object:  Table [dbo].[VersionType]    Script Date: 3/5/2020 8:57:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VersionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VersionTypeName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [datetime] NULL,
	[Rowstamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_VersionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[VersionType] ADD  CONSTRAINT [DF_VersionType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[VersionType] ADD  CONSTRAINT [DF_VersionType_CreatedBy]  DEFAULT ((1)) FOR [CreatedBy]
GO

ALTER TABLE [dbo].[VersionType] ADD  CONSTRAINT [DF_VersionType_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO


