IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckApiDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckApiDealer]
GO

	-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 20th Jan,2012
-- Description:	This Procdure will check i/p user is exists in TC_APIUsers aslo retun the dealerId
--              this procedure is used in Webservice
-- Modified By : Chetan Navin - 23/10/2015 (Added with(nolock)) 
-- Modified By : Suresh Prajapati on 11th Mar, 2016
-- Description : Removed Password Check
-- =============================================
CREATE PROCEDURE [dbo].[TC_CheckApiDealer] (
	@UserId VARCHAR(50) = NULL
	,@Password VARCHAR(50) = NULL
	,@DealerId BIGINT = NULL OUTPUT
	)
AS
BEGIN
	SELECT @DealerId = DealerId
	FROM TC_APIUsers WITH (NOLOCK)
	WHERE UserId = @UserId
		--AND Password = @Password
		AND IsActive = 1
END

