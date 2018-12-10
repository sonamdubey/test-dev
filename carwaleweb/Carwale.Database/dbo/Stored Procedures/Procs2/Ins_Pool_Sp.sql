IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Ins_Pool_Sp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Ins_Pool_Sp]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: 17-July-2008
-- Description:	SP to Insert/Update Ins_Pool Data
-- =============================================
CREATE PROCEDURE [dbo].[Ins_Pool_Sp] 
	-- Add the parameters for the stored procedure here
	@Id				NUMERIC, -- If @Id = -1 then Insert command else Update command
	@CustomerId		NUMERIC,
	@VersionId		NUMERIC,
	@MakeYear		DATETIME,
	@RegNo			VARCHAR(50),
	@InsuranceExp	DATETIME,
	@EntryDateTime	DATETIME,
	@Status			BIT OUTPUT -- 1 Insert and 0 for update
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @Id = -1
		BEGIN
			-- Insert statements for procedure here
			INSERT INTO Ins_Pool(CustomerId, VersionId, MakeYear, RegNo, InsuranceExp, EntryDateTime)
			VALUES(@CustomerId, @VersionId, @MakeYear, @RegNo, @InsuranceExp, @EntryDateTime)
			
			Set @Status = 1
		END
	ELSE
		BEGIN
			-- Update statements for procedure here
			Update Ins_Pool Set VersionId = @VersionId, MakeYear = @MakeYear, 
			RegNo = @RegNo, InsuranceExp = @InsuranceExp, UpdateDateTime = GetDate()
			WHERE Id = @Id
			
			Set @Status = 0
		END
END


