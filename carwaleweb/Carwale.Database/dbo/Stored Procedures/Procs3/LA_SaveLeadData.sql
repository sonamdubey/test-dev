IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LA_SaveLeadData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LA_SaveLeadData]
GO

	CREATE PROCEDURE [dbo].[LA_SaveLeadData]

	@Id						Numeric,
	@FirstName				VarChar(100),
	@LastName				VarChar(100),
	@Email					VarChar(100),
	@Mobile					VarChar(50),
	@Landline				VarChar(50),
	@CityId					Numeric,
	@CityName				VarChar(50),
	@MakeId					Numeric,
	@VersionId				Numeric,
	@CarName				VarChar(150),
	@ExpectedBuyingDate		DateTime,
	@LAId					Numeric,
	@ReferenceId			VarChar(50),
	@EntryDateTime			DateTime,
	@CRMLeadId				Numeric,
	@Status					SmallInt,
	@IsTesting				Bit,
	@LeadSource				VarChar(100),
	@NewLALeadId			Numeric OutPut,
	@PageName				Varchar(50) = NULL,
	@utm_source				Varchar(100) = NULL,
	@utm_medium				Varchar(100) = NULL,
	@utm_content			Varchar(100) = NULL,
	@utm_campaign			Varchar(100) = NULL
				
 AS
	
BEGIN
	SET @NewLALeadId = -1
	IF @Id = -1

		BEGIN

			INSERT INTO LA_Leads
			(
				FirstName, LastName, Email, Mobile, Landline, CityId, CityName,
				VersionId, CarName, ExpectedBuyingDate, LAId, ReferenceId, EntryDateTime,
				CRMLeadId, Status, IsTesting, MakeId, LeadSource,
				utm_source,utm_medium,utm_content,utm_campaign,PageName
			)
			VALUES
			(
				@FirstName, @LastName, @Email, @Mobile, @Landline, @CityId, @CityName,
				@VersionId, @CarName, @ExpectedBuyingDate, @LAId, @ReferenceId, @EntryDateTime,
				@CRMLeadId, @Status, @IsTesting, @MakeId, @LeadSource,
				@utm_source,@utm_medium,@utm_content,@utm_campaign,@PageName
			)
			
			SET @NewLALeadId = SCOPE_IDENTITY()
		
		END
END














