IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MapingOutlets_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MapingOutlets_SP]
GO

	


-- =============================================
-- Author:		Binumon George
-- Create date: 25-07-2011
-- Description:	This for maping Outlets from Opr.
-- =============================================
CREATE PROCEDURE [dbo].[TC_MapingOutlets_SP] 
	-- Add the parameters for the stored procedure here
	@DealerAdminId INT,
	@Status INT OUTPUT,
	@DealerId INT
	
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET @Status=0 -- Default staus will be 0.Insertion or updation not happened status will be 0
    -- Insert statements for procedure here
	-- checking adminID available or not.if not inserting data
	IF NOT EXISTS(SELECT top 1 Id FROM TC_DealerAdminMapping WHERE DealerId = @DealerId )
		BEGIN
			INSERT INTO TC_DealerAdminMapping(DealerAdminId,DealerId)
			VALUES(@DealerAdminId,@DealerId)
			SET @Status=1
		END
	ELSE
		BEGIN
			UPDATE TC_DealerAdminMapping SET DealerAdminId = @DealerAdminId WHERE DealerId = @DealerId
			SET @Status=1
		END
END


