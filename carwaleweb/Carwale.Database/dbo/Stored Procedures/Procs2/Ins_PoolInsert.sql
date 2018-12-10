IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Ins_PoolInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Ins_PoolInsert]
GO

	-- =============================================
-- Author:		Satish Sharma
-- Create date: Aug 12,2008 3:20 PM
-- Description:	This SP will dump data to Ins_Poos table
-- =============================================
CREATE PROCEDURE [dbo].[Ins_PoolInsert]
	-- Add the parameters for the stored procedure here
	@CustomerId			NUMERIC,	-- id of the owner of the car
	@VersionId			NUMERIC,	-- car version id
	@MakeYear			DateTime,	-- 
	@RegNo				VarChar(50),	-- reg number of the car
	@InsuranceExp		DateTime,	-- Insurance expiry date
	@SourceId			Int,
	@EntryDateTime		DateTime	-- Time and Date of Entry
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select Id FROM Ins_Pool WHERE CustomerId = @CustomerId AND VersionId = @VersionId
	
	IF @@RowCount = 0
		BEGIN
			Insert InTo Ins_Pool(CustomerId, VersionId, MakeYear, RegNo, InsuranceExp, EntryDateTime, SourceId)
			Values(@CustomerId, @VersionId, @MakeYear, @RegNo, @InsuranceExp, @EntryDateTime, @SourceId)
		END
END




