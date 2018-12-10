IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SkodaCampaignSP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SkodaCampaignSP]
GO

	-- =============================================

-- Author:		Mihir A chheda

-- Create date: 6-5-2014

-- Description:	Save data for Skoda Campaign

-- =============================================

CREATE PROCEDURE [dbo].[OLM_SkodaCampaignSP]

	@ServiceCenterId	NUMERIC,

	@FullName			VARCHAR(150),

	@Email				VARCHAR(100),

	@Mobile				VARCHAR(15),

	@City				NUMERIC,

	@VehicleRegNum		VARCHAR(15) = NULL,

	@Source             VARCHAR(50),

	@IPAddress			VARCHAR(20) = NULL,

	@IsMailSent			BIT = FALSE,

	@EntryDate			DATETIME = GETDATE

AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;



    -- Insert statements for procedure here

	INSERT INTO OLM_SkodaCampaign (ServiceCenterId,FullName,Email,Mobile,City,VehicleRegNum,Source,IPAddress,IsMailSent,EntryDate)

	VALUES (@ServiceCenterId,@FullName,@Email,@Mobile,@City,@VehicleRegNum,@Source,@IPAddress,@IsMailSent,@EntryDate)

END
