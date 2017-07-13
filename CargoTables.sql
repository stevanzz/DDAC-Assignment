CREATE TABLE [dbo].[Shipment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
	[DepartureLocation] [nvarchar](100) NOT NULL,
	[ArrivalLocation] [nvarchar](100) NOT NULL,
	[ContainerID] [tinyint] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[Days] [int] NOT NULL
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
 (
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
) ON [PRIMARY]


GO
SET IDENTITY_INSERT [dbo].[Shipment] ON 

declare @attendDate1 datetime, @attendDate2 datetime, @attendDate3 datetime
SET @attendDate1 = GETDATE()
SET @attendDate2 = GETDATE()
SET @attendDate3 = GETDATE()

Set @attendDate1 = DATEADD (mm, 1, @attendDate1)
Set @attendDate2 = DATEADD (mm, 2, @attendDate2)
Set @attendDate3 = DATEADD (mm, 3, @attendDate3)

INSERT [dbo].[Shipment] ([Id], [Title], [Description], [DepartureLocation], [ArrivalLocation], [ContainerID], [StartDate], [Days]) VALUES (1, N'Introductory Troubleshooting with Sysinternals Tools', N'Learn how to use tools such as Process Monitor to troubleshoot common problems.', N'Redmond', N'Indonesia', 0, @attendDate1, 3)
INSERT [dbo].[Shipment] ([Id], [Title], [Description], [DepartureLocation], [ArrivalLocation], [ContainerID], [StartDate], [Days]) VALUES (2, N'Windows Azure IaaS Virtual Machines and Virtual Networks', N'Learn how to build and configure Virtual Machines and Virtual Networks in the Cloud.', N'Orlando', N'Malaysia', 2, @attendDate2, 1)
INSERT [dbo].[Shipment] ([Id], [Title], [Description], [DepartureLocation], [ArrivalLocation], [ContainerID], [StartDate], [Days]) VALUES (3, N'What’s new in ASP.NET', N'Learn some of the new functionality in the next release of ASP.NET.', N'Redmond', N'Singapore', 1,  @attendDate3, 4)
