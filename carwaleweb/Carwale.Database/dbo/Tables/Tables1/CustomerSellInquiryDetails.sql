CREATE TABLE [dbo].[CustomerSellInquiryDetails] (
    [InquiryId]               NUMERIC (18)  NOT NULL,
    [RegistrationPlace]       VARCHAR (50)  NULL,
    [Insurance]               VARCHAR (50)  NULL,
    [InsuranceExpiry]         DATETIME      NULL,
    [Owners]                  NUMERIC (10)  CONSTRAINT [DF_CustomerSellInquiryDetails_Owners] DEFAULT ((1)) NULL,
    [Tax]                     VARCHAR (50)  NULL,
    [InteriorColor]           VARCHAR (50)  NULL,
    [InteriorColorCode]       VARCHAR (6)   NULL,
    [CityMileage]             VARCHAR (50)  NULL,
    [AdditionalFuel]          VARCHAR (50)  NULL,
    [CarDriven]               VARCHAR (50)  NULL,
    [Accidental]              BIT           CONSTRAINT [DF_CustomerSellInquiryDetails_Accidental] DEFAULT ((0)) NULL,
    [FloodAffected]           BIT           CONSTRAINT [DF_CustomerSellInquiryDetails_FloodAffected] DEFAULT ((0)) NULL,
    [Accessories]             VARCHAR (500) NULL,
    [Warranties]              VARCHAR (500) NULL,
    [Modifications]           VARCHAR (500) NULL,
    [ACCondition]             VARCHAR (50)  NULL,
    [BatteryCondition]        VARCHAR (50)  NULL,
    [BrakesCondition]         VARCHAR (50)  NULL,
    [ElectricalsCondition]    VARCHAR (50)  NULL,
    [EngineCondition]         VARCHAR (50)  NULL,
    [ExteriorCondition]       VARCHAR (50)  NULL,
    [SeatsCondition]          VARCHAR (50)  NULL,
    [SuspensionsCondition]    VARCHAR (50)  NULL,
    [TyresCondition]          VARCHAR (50)  NULL,
    [InteriorCondition]       VARCHAR (50)  NULL,
    [OverallCondition]        VARCHAR (50)  NULL,
    [Features_SafetySecurity] VARCHAR (200) NULL,
    [Features_Others]         VARCHAR (200) NULL,
    [Features_Comfort]        VARCHAR (200) NULL,
    [YoutubeVideo]            VARCHAR (200) NULL,
    [IsYouTubeVideoApproved]  BIT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CustomerSellInquiryDetails] PRIMARY KEY CLUSTERED ([InquiryId] ASC) WITH (FILLFACTOR = 90)
);


GO


-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <23/12/2013>
-- Description:	<Update VideoCount in Livelistings whenever the IsYouTubeVideoApproved is updated>
-- Modified By Reshma Shetty 24/12/2013 to support bulk updates	
-- =============================================
CREATE TRIGGER [dbo].[TR_AU_IndividualVideoCount] 
   ON [dbo].[CustomerSellInquiryDetails]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	IF(UPDATE(IsYouTubeVideoApproved))
	BEGIN
	--DECLARE @InquiryId INT,@VideoCount BIT
	--SELECT @InquiryId=InquiryId,@VideoCount=IsYouTubeVideoApproved FROM Inserted

	--This logic will have to be changed in case the facility to upload more than 1 video is given to the individual
	
	--UPDATE Livelistings
	--SET VideoCount= @VideoCount
	--WHERE SellerType=2 AND InquiryId=@InquiryId
	
	--Modified to support bulk updates	
	UPDATE LL
	SET VideoCount= ID.IsYouTubeVideoApproved
	FROM Inserted as ID
	INNER JOIN Livelistings as LL ON LL.Inquiryid=ID.InquiryId AND SellerType=2
	

	END

END


