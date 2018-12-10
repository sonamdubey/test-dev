IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveRejectedCarReasons]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveRejectedCarReasons]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 18th May 2015
-- Description:	To save rejected reasons for absure car
-- Updated By  : Vinay Kumar Prajapati 22 may 2015
-- Updated By  : Vinay Kumar Prajapati 26 may 2015  call sp "AbSure_SaveWarrantyType"
-- Modified By : Suresh Prajapati on 06th July, 2015
-- Description : To Save Rejection Reason Comments
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveRejectedCarReasons] @AbSure_CarDetailsId INT
	,@RejectionMethod INT
	,@WarrantyGivenBy INT = NULL
	---Login User Id 
	,@RejectedReasons [dbo].[AbSure_RejectedReasonsTblTyp] READONLY
	,@RejectionComments VARCHAR(500) = NULL
AS
BEGIN
	UPDATE AbSure_CarDetails
	SET RejectionMethod = @RejectionMethod
		,RejectionComments = @RejectionComments -- Added By Suresh Prajapati on 06th July, 2015
		,IsRejected = 1, RejectedDateTime = GETDATE()	
	WHERE Id = @AbSure_CarDetailsId

	INSERT INTO Absure_RejectedCarReasons (
		Absure_CarDetailsId
		,RejectedType
		,RejectedReason
		)
	SELECT @AbSure_CarDetailsId
		,Type
		,Reasons
	FROM @RejectedReasons

	IF @RejectionMethod = 2 -- For manual Rejection  
	BEGIN
		EXEC AbSure_SaveWarrantyType NULL
			,@WarrantyGivenBy
			,@AbSure_CarDetailsId
			,0
			--UPDATE AbSure_CarDetails SET RejectionMethod = @RejectionMethod,IsRejected=1 ,RejectedDateTime =GETDATE() WHERE Id=@AbSure_CarDetailsId
	END
END

