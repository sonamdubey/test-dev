IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveTataLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveTataLead]
GO

	
-- =============================================
-- Author:		Vaibhav K
-- Create date: 2 June 2014
-- Description:	Save Tata Cardekho lead or update existing record and return id / else return -1
-- Modifier   : Vinay Kumar prajapati 26 Jun 2014 update name email mobile no.
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveTataLead]
	-- Add the parameters for the stored procedure here
	@Id					NUMERIC(18,0) = -1,
	@FirstName			VARCHAR(100) = NULL,
	@LastName			VARCHAR(50) = NULL,
	@Mobile				VARCHAR(15) = NULL,
	@City				VARCHAR(20) = NULL,
	@State				VARCHAR(20) = NULL,
	@Address			VARCHAR(500) = NULL,
	@Pincode			VARCHAR(10) = NULL,
	@Model				VARCHAR(20) = NULL,
	@Variant			VARCHAR(50) = NULL,
	@DealerDivisionId	VARCHAR(50) = NULL,
	@Email				VARCHAR(50) = NULL,
	@IsPushSuccess		BIT = 0,
	@MessageReturned	VARCHAR(400) = NULL,
	@CBDId				NUMERIC(18,0) = NULL,
	@UpdatedOn			DATETIME = NULL,
	@UpdatedBy			INT = NULL,
	@OutputId			NUMERIC(18,0) = -1 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @Id = -1
		BEGIN
			INSERT INTO CRM_TataLeads(FirstName, LastName, Mobile, City, State, Address, Pincode, Model, Variant, DealerDivisionId, Email, IsPushSuccess, MessageReturned, CBDId)
			VALUES(@FirstName, @LastName, @Mobile, @City, @State, @Address, @Pincode, @Model, @Variant, @DealerDivisionId, @Email, @IsPushSuccess, @MessageReturned, @CBDId)

			SET @OutputId = SCOPE_IDENTITY()
		END
	ELSE
		BEGIN
			UPDATE CRM_TataLeads
				SET 
				    FirstName = CASE WHEN (@FirstName is null) THEN FirstName ELSE @FirstName END,
					LastName = CASE WHEN (@LastName is null) THEN LastName ELSE @LastName END,
					Email = CASE WHEN (@Email is null) THEN Email ELSE @Email END,
					Mobile = CASE WHEN (@Mobile is null) THEN Mobile ELSE @Mobile END,
					Model = CASE WHEN (@Model is null) THEN Model ELSE @Model END,
					Variant = CASE WHEN (@Variant is null) THEN Variant ELSE @Variant END,
					City = CASE WHEN (@City is null) THEN City ELSE @City END,
					Pincode = CASE WHEN (@Pincode is null) THEN Pincode ELSE @Pincode END,
					DealerDivisionId = CASE WHEN (@DealerDivisionId is null) THEN DealerDivisionId ELSE @DealerDivisionId END,
					IsPushSuccess = @IsPushSuccess,
					MessageReturned = @MessageReturned,
					UpdatedOn = @UpdatedOn,
					UpdatedBy = @UpdatedBy
			WHERE Id = @Id

			SET @OutputId = @Id
		END
END
