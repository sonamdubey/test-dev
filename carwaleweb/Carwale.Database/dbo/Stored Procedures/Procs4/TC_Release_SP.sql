IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Release_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Release_SP]
GO

	-- =============================================
-- ModifiedBy:		Binumon George
-- Modified date:   21-12-2011
-- Description: Added parameter @DisplayDate and used in insert and update status
-- =============================================
-- ModifiedBy:		Binumon George
-- Modified date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modi
-- =============================================
-- Author:		Binumon George
-- Create date: 05-10-2011
-- Description:	Entering tc updates from Opr.
-- =============================================
CREATE PROCEDURE [dbo].[TC_Release_SP]
	-- Add the parameters for the stored procedure here
	@TC_ReleaseId INT=NULL,
	@Content VARCHAR(1000),
	@DisplayDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    IF(@TC_ReleaseId IS NULL)
		BEGIN
			INSERT INTO TC_Release(Content,DisplayDate)values(@Content,@DisplayDate)
		END
	ELSE
		BEGIN
			UPDATE TC_Release SET Content=@Content,DisplayDate=@DisplayDate, ModifiedDate=GETDATE() WHERE TC_Release_Id=@TC_ReleaseId
		END
		--set @Status=1	
END

