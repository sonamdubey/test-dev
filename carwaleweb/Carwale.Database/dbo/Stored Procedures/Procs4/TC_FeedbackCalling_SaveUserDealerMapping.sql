IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FeedbackCalling_SaveUserDealerMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FeedbackCalling_SaveUserDealerMapping]
GO

	
-- =============================================
-- Author:		Ruchira Patil
-- Create date: 26th Sept 2016
-- Description:	to save dealer-user mapping
-- Modified By : Khushaboo Patil on 3 rd oct 2016 added if exists instead of checking rowcount and added output parameter 
--TC_FeedbackCalling_SaveUserDealerMapping '4' ,88939 
-- =============================================
create PROCEDURE [dbo].[TC_FeedbackCalling_SaveUserDealerMapping]
	@DealerId VARCHAR(max),
	@UserId INT,
	@DealermappingId INT OUTPUT
AS
BEGIN
	-- Modified By : Khushaboo Patil on 3 rd oct 2016
	IF EXISTS(SELECT TC_FeedbackCalling_DealerMappingId FROM TC_FeedbackCalling_Dealermapping WITH (NOLOCK) WHERE UserId = @UserId)
	BEGIN
		DELETE FROM TC_FeedbackCalling_Dealermapping WHERE UserId = @UserId AND IsActive = 1
	END

	INSERT INTO TC_FeedbackCalling_Dealermapping (UserId,DealerId,EntryDate,IsActive) 
	SELECT @UserId,ListMember,GETDATE(),1 FROM fnSplitCSV(@DealerId)

	-- Modified By : Khushaboo Patil on 3 rd oct 2016
	SET @DealermappingId = SCOPE_IDENTITY()
END


