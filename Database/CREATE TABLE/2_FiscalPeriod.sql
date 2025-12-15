USE [AE_Challange]
GO

/****** Object:  Table [dbo].[FiscalPeriod]    Script Date: 12/15/2025 5:25:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FiscalPeriod](
	[PeriodID] [int] NOT NULL,
	[FiscalYear] [int] NOT NULL,
	[PeriodMonth] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[UpdatedDate] [datetime2](3) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedBy] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PeriodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_FiscalPeriod] UNIQUE NONCLUSTERED 
(
	[FiscalYear] ASC,
	[PeriodMonth] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FiscalPeriod] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[FiscalPeriod] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

