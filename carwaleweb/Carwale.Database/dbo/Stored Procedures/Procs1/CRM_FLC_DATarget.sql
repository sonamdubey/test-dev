IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FLC_DATarget]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FLC_DATarget]
GO

	-- =============================================
-- Author      : Vinay Kumar Prajapati
-- Create date : 30 July 2013
-- Description : Save FLC_Leads DataAssign GroupType=1 for OEM, 2 For NCD, 3 For HS000 
-- =============================================
CREATE PROCEDURE [dbo].[CRM_FLC_DATarget]
(
@OEM         BIGINT,
@NCD         BIGINT,
@HS000       BIGINT,
@CreatedBy   BIGINT,
@CreatedOn   DateTime,
@Date        DateTime
)
AS
   BEGIN    
       SELECT ID FROM CRM_DATarget AS CDT WITH(NOLOCK) WHERE CDT.GroupType=1 AND CDT.Date= @Date
       IF @@ROWCOUNT = 0 AND @OEM <>-1
		   BEGIN
			  INSERT INTO CRM_DATarget(GroupType,Date,CreatedBy,CreatedOn,DA_Target) VALUES(1,@Date,@CreatedBy,@CreatedOn,@OEM)
		   END
		   
	 SELECT ID FROM CRM_DATarget AS CDT WITH(NOLOCK) WHERE  CDT.GroupType=2 AND CDT.Date= @Date
       IF @@ROWCOUNT = 0 AND @NCD <>-1	  
		   BEGIN
			  INSERT INTO CRM_DATarget(GroupType,Date,CreatedBy,CreatedOn,DA_Target) VALUES(2,@Date,@CreatedBy,@CreatedOn,@NCD)
		   END   
		   		      
     SELECT ID FROM CRM_DATarget AS CDT WITH(NOLOCK) WHERE CDT.GroupType=3 AND CDT.Date= @Date
      IF @@ROWCOUNT = 0 AND @HS000 <>-1		  
		   BEGIN
			  INSERT INTO CRM_DATarget(GroupType,Date,CreatedBy,CreatedOn,DA_Target) VALUES(3,@Date,@CreatedBy,@CreatedOn,@HS000)
		   END   
 END 