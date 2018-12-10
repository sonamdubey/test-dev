IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_InsertUsedCarOpportunity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_InsertUsedCarOpportunity]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 9 June 2014
-- Description:	Save Used Car Opportunity (lead interested in used car) and return id
-- =============================================
CREATE PROCEDURE [dbo].[CRM_InsertUsedCarOpportunity]
	-- Add the parameters for the stored procedure here
	@LeadId			NUMERIC(18,0),
	@Budget			VARCHAR(10),
	@MakeId			INT,
	@ModelId		INT,
	@CreatedBy		INT,
	@Id				NUMERIC(18,0) OUTPUT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SET @Id = -1
	
	SELECT Id FROM CRM_UsedCarOpportunity WHERE LeadId = @LeadId AND Budget = @Budget AND MakeId = @MakeId AND ModelId = @ModelId

	IF @@ROWCOUNT > 0
		BEGIN
			SET @Id = 0
		END
	ELSE IF @LeadId <> -1 AND @LeadId IS NOT NULL
		BEGIN
			INSERT INTO CRM_UsedCarOpportunity (LeadId, Budget, MakeId, ModelId, CreatedBy)
			VALUES (@LeadId, @Budget, @MakeId, @ModelId, @CreatedBy)
			
			SET @Id = SCOPE_IDENTITY()
		END

END
