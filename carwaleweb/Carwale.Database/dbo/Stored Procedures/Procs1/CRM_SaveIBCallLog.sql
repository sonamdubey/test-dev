IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveIBCallLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveIBCallLog]
GO

	


CREATE PROCEDURE [dbo].[CRM_SaveIBCallLog]

	@Id					Numeric,
	@Type				SmallInt,
	@MobileNumber		VarChar(20),
	@Source				Numeric,
	@SubSource  		Numeric,
	@IsUsedCar			Bit,
	@IsNewCar			Bit,
	@NCLeadId			Numeric,
	@SellInquiryId		Numeric,
	@LeadBy				Numeric,
	@CurrentId			Numeric OutPut	
				
 AS
	
BEGIN
	SET @CurrentId = -1
	
	IF @Id = -1 

		BEGIN

			INSERT INTO CRM_IBCallLog
			(
				MobileNumber, Source, LeadBy, SubSource
			)
			VALUES
			(
				@MobileNumber, @Source, @LeadBy, @SubSource
			)
			
			SET @CurrentId = SCOPE_IDENTITY()
		
		END

	ELSE
		
		BEGIN
			IF @Type = 1
				BEGIN
					UPDATE CRM_IBCallLog SET IsNewCar = @IsNewCar,	NCLeadId = @NCLeadId
					WHERE ID = @Id
				END
			ELSE
				BEGIN
					UPDATE CRM_IBCallLog SET IsUsedCar = @IsUsedCar, SellInquiryId = @SellInquiryId
					WHERE ID = @Id
				END
		END
END















