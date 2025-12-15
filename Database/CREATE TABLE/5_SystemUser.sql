USE [AE_Challange]
GO

/****** Object:  Table [dbo].[SystemUser]    Script Date: 12/15/2025 5:25:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SystemUser](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[UserRole] [nvarchar](50) NOT NULL,
	[PasswordHash] [varbinary](128) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK__SystemUs__1788CC4CB27AB7B9] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__SystemUs__C9F284567F43C5F3] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF__SystemUse__IsAct__32767D0B]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[SystemUser] ADD  CONSTRAINT [DF__SystemUse__Creat__336AA144]  DEFAULT (getdate()) FOR [CreatedDate]
GO

