IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveJDLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveJDLeads]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 30 Nov 2012
-- Description:	Save the Just Dial Audi lead data into database (Table: CRM_JDLeads)
-- =============================================
CREATE PROCEDURE [dbo].[CRM_SaveJDLeads]
	-- Add the parameters for the stored procedure here
	@LeadId		VARCHAR(20),
	@LeadType	VARCHAR(15),
	@Prefix		VARCHAR(5),
	@Name		VARCHAR(100),
	@Mobile		VARCHAR(15),
	@Phone		VARCHAR(15),
	@Email		VARCHAR(50),
	@Date		VARCHAR(50),
	@Category	VARCHAR(100),
	@City		VARCHAR(50),
	@Area		VARCHAR(50),
	@BranchArea	VARCHAR(100),
	@DncMobile	VARCHAR(5),
	@DncPhone	VARCHAR(5),
	@Company	VARCHAR(100),
	@Type		varchar(30),
	@Id			INT OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @Id = -1

    -- Insert statements for procedure here
	INSERT INTO CRM_JDLeads
		(
			LeadId,LeadType,Prefix,Name,Mobile,Phone,Email,Date,Category,City,Area,
			BranchArea,DncMobile,DncPhone,Company,Type
		)
	VALUES
		(
			@LeadId,@LeadType,@Prefix,@Name,@Mobile,@Phone,@Email,@Date,@Category,@City,@Area,
			@BranchArea,@DncMobile,@DncPhone,@Company,@Type			
		)
		
	SET @Id = SCOPE_IDENTITY()
END
