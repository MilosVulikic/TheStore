using System;
using System.Collections.Generic;
using System.Reflection;
using TheShop.Controllers;
using TheShop.DAL.Interfaces;
using TheShop.DAL.Models;
using TheShop.DAL.Repositories;
using TheShop.DTOs;
using TheShop.Mappers;
using TheShop.Services;
using TheShop.Services.Interfaces;

namespace TheShop
{
	public sealed class Startup
	{
		private static Startup _instance = null;		
		Dictionary<Type, Type> _typeMapper = null;
		Dictionary<Type, ConstructorInfo> _constructorMapper = null;

		public static Startup Instance 
		{
			get
			{
				if (_instance == null)
					_instance = new Startup();
				return _instance;
			}
		}

		private Startup()
		{
			_typeMapper = new Dictionary<Type, Type>();
			_typeMapper.Add(typeof(IShopService),typeof(ShopService));
			_typeMapper.Add(typeof(ISupplier), typeof(SupplierService));
			_typeMapper.Add(typeof(ISupplierService), typeof(SupplierService));
			_typeMapper.Add(typeof(IArticleRepository), typeof(ArticleRepository));
			_typeMapper.Add(typeof(IMapper<Article,ArticleDTO>), typeof(ArticleMapper));
		
			_constructorMapper = new Dictionary<Type, ConstructorInfo>();
			_constructorMapper.Add(typeof(ShopController), typeof(ShopController).GetConstructor(new Type[] { typeof(IShopService), typeof(IMapper<Article, ArticleDTO>) }));			
			_constructorMapper.Add(typeof(ShopService), typeof(ShopService).GetConstructor(new Type[] { typeof(IArticleRepository), typeof(ISupplierService) }));			
		}
		

		/// <summary>
		/// Instantiate the object of type using parametrized constructror.
		/// Note: this version works first defined constructor in a class
		/// PLEASE: Make sure not to have Parent types as property of children or its' children classes to avoid neverending recursion
		/// </summary>
		/// <param name="type"></param>
		/// <returns>Object of type provided as input parameter</returns>
		public T Instantiator<T>()
		{
			var currentType = GetConcreteType(typeof(T));
			return (T)Instantiator(currentType);
		}


		private object Instantiator(Type type)	// NOTE for myself: Recursive methods should not use generics
		{
			var currentType = GetConcreteType(type);

			List<ConstructorInfo> specifiedCtors = GetSpecifiedConstructorsForType(currentType);

			ConstructorInfo[] ctors;
			if (specifiedCtors == null || specifiedCtors.Count == 0)
				ctors = currentType.GetConstructors();
			else
				ctors = specifiedCtors.ToArray();

			var ctor = ctors[0];

			var ctorNumberOfParameters = ctor.GetParameters().Length;
			List<object> constructorParameters = new List<object>();

			Console.WriteLine($"[Instantiatior] - Calling Constructor with {ctorNumberOfParameters} parameters - type: {currentType.Name}");
			if (ctorNumberOfParameters > 0)
			{				
				var parameterTypes = new List<Type>();
				foreach (var parameter in ctor.GetParameters())
				{
					constructorParameters.Add(Instantiator(parameter.ParameterType));
				}
			}


			if (constructorParameters.Count > 1)
			{
				Console.WriteLine($"[Instantiatior] Returning object of type: {currentType.Name}, constructed with {constructorParameters.Count} input parameters");
				return Activator.CreateInstance(currentType, constructorParameters.ToArray());
			}				
			else if (constructorParameters.Count == 1)
			{
				Console.WriteLine($"[Instantiatior] Returning object of type: {currentType.Name}, constructed with {constructorParameters.Count} input parameters");
				return Activator.CreateInstance(currentType, constructorParameters[0]);
			}				
			else
			{
				Console.WriteLine($"[Instantiatior] Returning object of type: {currentType.Name}, constructed without input parameters");
				return Activator.CreateInstance(currentType);
			}
				
		}

		private List<ConstructorInfo> GetSpecifiedConstructorsForType(Type currentType)
		{
			var specifiedCtor = GetConstructorOfType(currentType);
			List<ConstructorInfo> specifiedCtors = null;
			if (specifiedCtor != null)
				specifiedCtors = new List<ConstructorInfo> { specifiedCtor };
			return specifiedCtors;
		}	

		private Type GetConcreteType(Type type)
		{
			Type result = type;
			if (type.IsInterface)
			{
				result = _typeMapper[type];
			}
			return result;
		}
		
		public void MapInterfaceToConcreteType(Type interfaceType, Type type)
		{
			if (!_typeMapper.ContainsKey(interfaceType))
			{
				_typeMapper.Add(interfaceType, type);
			}			
		}

		public ConstructorInfo GetConstructorOfType(Type type)
		{
			
			if (_constructorMapper.ContainsKey(type))
			{
				return _constructorMapper[type];
			}
			return null;
		}

		public void MapTypeToConstructor(Type type, ConstructorInfo constructor)
		{
			if (!_constructorMapper.ContainsKey(type))
			{
				_constructorMapper.Add(type, constructor);
			}
		}
		
	}


}