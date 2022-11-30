using System;
using System.Collections.Generic;
using BansheeGz.BGDatabase;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

#pragma warning disable 414

public partial class TBL_CHAT : BGEntity
{

	public class Factory : BGEntity.EntityFactory
	{
		public BGEntity NewEntity(BGMetaEntity meta) => new TBL_CHAT(meta);
		public BGEntity NewEntity(BGMetaEntity meta, BGId id) => new TBL_CHAT(meta, id);
	}
	private static BansheeGz.BGDatabase.BGMetaRow _metaDefault;
	public static BansheeGz.BGDatabase.BGMetaRow MetaDefault => _metaDefault ?? (_metaDefault = BGCodeGenUtils.GetMeta<BansheeGz.BGDatabase.BGMetaRow>(new BGId(5099863351450953490UL,7936239507720578980UL), () => _metaDefault = null));
	public static BansheeGz.BGDatabase.BGRepoEvents Events => BGRepo.I.Events;
	public static int CountEntities => MetaDefault.CountEntities;
	public System.String name
	{
		get => _name[Index];
		set => _name[Index] = value;
	}
	public System.String Nickname
	{
		get => _Nickname[Index];
		set => _Nickname[Index] = value;
	}
	public System.String Message
	{
		get => _Message[Index];
		set => _Message[Index] = value;
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName _ufle12jhs77_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _name => _ufle12jhs77_name ?? (_ufle12jhs77_name = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldEntityName>(MetaDefault, new BGId(5235475840445975572UL, 7814373756752145589UL), () => _ufle12jhs77_name = null));
	private static BansheeGz.BGDatabase.BGFieldString _ufle12jhs77_Nickname;
	public static BansheeGz.BGDatabase.BGFieldString _Nickname => _ufle12jhs77_Nickname ?? (_ufle12jhs77_Nickname = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldString>(MetaDefault, new BGId(5699304523372681538UL, 10272742320159725735UL), () => _ufle12jhs77_Nickname = null));
	private static BansheeGz.BGDatabase.BGFieldString _ufle12jhs77_Message;
	public static BansheeGz.BGDatabase.BGFieldString _Message => _ufle12jhs77_Message ?? (_ufle12jhs77_Message = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldString>(MetaDefault, new BGId(5092513450800360305UL, 3154131771800230304UL), () => _ufle12jhs77_Message = null));
	private static readonly TBL_CHAT.Factory _factory0_PFS = new TBL_CHAT.Factory();
	private TBL_CHAT() : base(MetaDefault)
	{
	}
	private TBL_CHAT(BGId id) : base(MetaDefault, id)
	{
	}
	private TBL_CHAT(BGMetaEntity meta) : base(meta)
	{
	}
	private TBL_CHAT(BGMetaEntity meta, BGId id) : base(meta, id)
	{
	}
	public static TBL_CHAT FindEntity(Predicate<TBL_CHAT> filter)
	{
		return MetaDefault.FindEntity(entity => filter==null || filter((TBL_CHAT) entity)) as TBL_CHAT;
	}
	public static List<TBL_CHAT> FindEntities(Predicate<TBL_CHAT> filter, List<TBL_CHAT> result=null, Comparison<TBL_CHAT> sort=null) => BGCodeGenUtils.FindEntities(MetaDefault, filter, result, sort);
	public static void ForEachEntity(Action<TBL_CHAT> action, Predicate<TBL_CHAT> filter=null, Comparison<TBL_CHAT> sort=null)
	{
		MetaDefault.ForEachEntity(entity => action((TBL_CHAT) entity), filter == null ? null : (Predicate<BGEntity>) (entity => filter((TBL_CHAT) entity)), sort==null?(Comparison<BGEntity>) null:(e1,e2) => sort((TBL_CHAT)e1,(TBL_CHAT)e2));
	}
	public static TBL_CHAT GetEntity(BGId entityId) => (TBL_CHAT) MetaDefault.GetEntity(entityId);
	public static TBL_CHAT GetEntity(int index) => (TBL_CHAT) MetaDefault[index];
	public static TBL_CHAT GetEntity(string entityName) => (TBL_CHAT) MetaDefault.GetEntity(entityName);
	public static TBL_CHAT NewEntity() => (TBL_CHAT) MetaDefault.NewEntity();
	public static TBL_CHAT NewEntity(BGId entityId) => (TBL_CHAT) MetaDefault.NewEntity(entityId);
	public static TBL_CHAT NewEntity(Action<TBL_CHAT> callback)
	{
		return (TBL_CHAT) MetaDefault.NewEntity(new BGMetaEntity.NewEntityContext(entity => callback((TBL_CHAT)entity)));
	}
}
#pragma warning restore 414
