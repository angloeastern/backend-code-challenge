USE [AE_Challange]
GO

/****** Object:  Table [dbo].[ChartOfAccount]    Script Date: 12/15/2025 5:27:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ChartOfAccount](
	[COA_ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [nvarchar](10) NOT NULL,
	[AccountDescription] [nvarchar](100) NOT NULL,
	[ParentCOA_ID] [int] NULL,
	[AccountType] [nvarchar](10) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedBy] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[COA_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[AccountNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ChartOfAccount] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[ChartOfAccount] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[ChartOfAccount]  WITH CHECK ADD  CONSTRAINT [FK_COA_ParentCOA] FOREIGN KEY([ParentCOA_ID])
REFERENCES [dbo].[ChartOfAccount] ([COA_ID])
GO

ALTER TABLE [dbo].[ChartOfAccount] CHECK CONSTRAINT [FK_COA_ParentCOA]
GO

ALTER TABLE [dbo].[ChartOfAccount]  WITH CHECK ADD  CONSTRAINT [CHK_COA_AccountType] CHECK  (([AccountType]='Child' OR [AccountType]='Parent'))
GO

ALTER TABLE [dbo].[ChartOfAccount] CHECK CONSTRAINT [CHK_COA_AccountType]
GO

