IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_SaveCorporateBuy]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_SaveCorporateBuy]
GO

	-- =============================================
-- Author:		Vaibhav Kale
-- Create date: 23-Feb-2012
-- Description:	Inserts new record in OLM_CorporateBuy & OLM_CorporateBuyCars
-- =============================================
CREATE PROCEDURE [dbo].[OLM_SaveCorporateBuy]
	-- Add the parameters for the stored procedure here
	@FullName			VARCHAR(150),
	@Email				VARCHAR(100),
	@Mobile				VARCHAR(15),
	@Landline			VARCHAR(15),
	@Organisation		VARCHAR(50),
	@City				NUMERIC(18),
	@Address			VARCHAR(500),
	@PreferredDealer	NUMERIC(18),
	@Testdrive			BIT,
	@Message			VARCHAR(500),
	@ModelId			VARCHAR(200),
	@ModelCounts		VARCHAR(200),
	@IpAddress			VARCHAR(50),
	@LeaseCompany		NUMERIC(18)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @InquiryId NUMERIC(10)

    -- Insert statements for procedure here
	INSERT INTO OLM_CorporateBuy(FullName,Email,Mobile,Landline,Organisation,
				City,Address,PreferredDealer,Testdrive,Message,IpAddress,LeaseCompany)
				VALUES(@FullName,@Email,@Mobile,@Landline,@Organisation,
				@City,@Address,@PreferredDealer,@Testdrive,@Message,@IpAddress,@LeaseCompany)
	SET @InquiryId = SCOPE_IDENTITY()
	
	DECLARE @ModelIndx SMALLINT,@strModel VARCHAR(10),@QuantityIndx SMALLINT,@strQuantity VARCHAR(10)
	
	WHILE @ModelId <> ''
		BEGIN                
						SET @ModelIndx = CHARINDEX(',', @ModelId) 
						SET @QuantityIndx = CHARINDEX(',', @ModelCounts) 
						--to check if model id has ended                
						IF @ModelIndx>0                
							BEGIN 
									SET @strModel = LEFT(@ModelId, @ModelIndx-1)  
									SET @ModelId = RIGHT(@ModelId, LEN(@ModelId)-@ModelIndx) 
				                
									SET @strQuantity = LEFT(@ModelCounts, @QuantityIndx-1)   
									SET @ModelCounts = RIGHT(@ModelCounts, LEN(@ModelCounts)-@QuantityIndx) 
									
									INSERT INTO OLM_CorporateBuyCars(InquiryId,ModelId,ModelCounts)
									VALUES(@InquiryId,@strModel,@strQuantity)
							END                
						ELSE                
							BEGIN 
									SET @strModel = @ModelId  
									SET @ModelId = ''   
								                
									SET @strQuantity = @ModelCounts
												
									INSERT INTO OLM_CorporateBuyCars(InquiryId,ModelId,ModelCounts)
									VALUES(@InquiryId,@strModel,@ModelCounts)
							END
		END
END
