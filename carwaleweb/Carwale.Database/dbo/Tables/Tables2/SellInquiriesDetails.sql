CREATE TABLE [dbo].[SellInquiriesDetails] (
    [SellInquiryId]               NUMERIC (18)  NOT NULL,
    [Owners]                      VARCHAR (50)  CONSTRAINT [DF_SellInquiriesDetails_Owners] DEFAULT ((1)) NULL,
    [RegistrationPlace]           VARCHAR (50)  NULL,
    [OneTimeTax]                  VARCHAR (50)  NULL,
    [Insurance]                   VARCHAR (50)  NULL,
    [InsuranceExpiry]             DATETIME      NULL,
    [UpdateTimeStamp]             VARCHAR (100) NULL,
    [InteriorColor]               VARCHAR (50)  NULL,
    [InteriorColorCode]           VARCHAR (6)   NULL,
    [CityMileage]                 VARCHAR (50)  NULL,
    [AdditionalFuel]              VARCHAR (50)  NULL,
    [CarDriven]                   VARCHAR (50)  NULL,
    [Accidental]                  BIT           CONSTRAINT [DF_SellInquiriesDetails_Accidental] DEFAULT ((0)) NULL,
    [FloodAffected]               BIT           CONSTRAINT [DF_SellInquiriesDetails_FloodAffected] DEFAULT ((0)) NULL,
    [Warranties]                  VARCHAR (500) NULL,
    [Modifications]               VARCHAR (500) NULL,
    [ACCondition]                 VARCHAR (50)  NULL,
    [BatteryCondition]            VARCHAR (50)  NULL,
    [BrakesCondition]             VARCHAR (50)  NULL,
    [ElectricalsCondition]        VARCHAR (50)  NULL,
    [EngineCondition]             VARCHAR (50)  NULL,
    [ExteriorCondition]           VARCHAR (50)  NULL,
    [InteriorCondition]           VARCHAR (50)  NULL,
    [SeatsCondition]              VARCHAR (50)  NULL,
    [SuspensionsCondition]        VARCHAR (50)  NULL,
    [TyresCondition]              VARCHAR (50)  NULL,
    [OverallCondition]            VARCHAR (50)  NULL,
    [Features_SafetySecurity]     VARCHAR (200) NULL,
    [Features_Comfort]            VARCHAR (200) NULL,
    [Features_Others]             VARCHAR (200) NULL,
    [YoutubeVideo]                VARCHAR (200) NULL,
    [IsYouTubeVideoApproved]      BIT           DEFAULT ((0)) NULL,
    [InstalledFeaturesMissingIds] VARCHAR (500) NULL,
    CONSTRAINT [PK_SellInqDetails] PRIMARY KEY CLUSTERED ([SellInquiryId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SellInqDetails_SellInq] FOREIGN KEY ([SellInquiryId]) REFERENCES [dbo].[SellInquiries] ([ID]) ON UPDATE CASCADE
);


GO

-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <23/12/2013>
-- Description:	<Update VideoCount in Livelistings whenever the IsYouTubeVideoApproved is updated>
-- Modified By Reshma Shetty 24/12/2013 to support bulk updates	
-- Modified by Manish on 24-07-2014 added with (nolock) keyword
-- Modified By vivek gupta on 16-09-2015, chnaged trigger to fire in case of insertion to the table
-- =============================================
CREATE TRIGGER [dbo].[TR_AU_DealerVideoCount] 
   ON [dbo].[SellInquiriesDetails]
   FOR INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	--SELECT COUNT(SellInquiryId) FROM Inserted
	IF(UPDATE(IsYouTubeVideoApproved))
	BEGIN
	--DECLARE @InquiryId INT,@VideoCount BIT
	--SELECT @InquiryId=SellInquiryId,@VideoCount=IsYouTubeVideoApproved FROM Inserted

	--This logic will have to be changed in case the facility to upload more than 1 video is given to the dealer
	-- Modified By Reshma Shetty 24/12/2013 to support bulk updates	
	UPDATE LL
	SET VideoCount= ID.IsYouTubeVideoApproved
	FROM Inserted as ID
	INNER JOIN Livelistings as LL WITH (NOLOCK) ON LL.Inquiryid=ID.SellInquiryId AND SellerType=1
	--WHERE SellerType=1 AND InquiryId=@InquiryId
	END

END

