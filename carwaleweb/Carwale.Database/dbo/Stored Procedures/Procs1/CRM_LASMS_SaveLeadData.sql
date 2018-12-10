IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_LASMS_SaveLeadData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_LASMS_SaveLeadData]
GO

	

CREATE PROCEDURE [dbo].[CRM_LASMS_SaveLeadData]

	@Id						Numeric,
	@FirstName				VarChar(100),
	@LastName				VarChar(100),
	@Email					VarChar(100),
	@Mobile					VarChar(50),
	@CityId					Numeric,
	@VersionId				Numeric,
	@ExpectedBuyingDate		DateTime,
	@LAId					Numeric,
	@Status					SmallInt,
	@Category				VarChar(100),
	@LeadSource				VarChar(200),
	@CRMLeadId				Numeric,
	@EntryDateTime			DateTime,
	@NewLALeadId			Numeric OutPut	
				
 AS
	
BEGIN
	SET @NewLALeadId = -1
	IF @Id = -1

		BEGIN

			INSERT INTO CRM_LASMSLeads
			(
				FirstName, LastName, Email, Mobile, CityId,
				VersionId, ExpectedBuyingDate, LAId, Status,
				Category, LeadSource, CRMLeadId, EntryDateTime
			)
			VALUES
			(
				@FirstName, @LastName, @Email, @Mobile, @CityId,
				@VersionId, @ExpectedBuyingDate, @LAId, @Status,
				@Category, @LeadSource, @CRMLeadId, @EntryDateTime
			)
			
			SET @NewLALeadId = SCOPE_IDENTITY()
		
		END
END

