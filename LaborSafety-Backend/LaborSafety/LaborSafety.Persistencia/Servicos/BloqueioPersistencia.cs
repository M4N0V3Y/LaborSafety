using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using LaborSafety.Persistencia.Interfaces;
using LaborSafety.Utils.Constantes;

namespace LaborSafety.Persistencia.Servicos
{
    public class BloqueioPersistencia : IBloqueioPersistencia
    {
        public BLOQUEIO InserirBloqueio(BLOQUEIO bloqueio, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var bloq = new BLOQUEIO()
            {
                CodArea = bloqueio.CodArea == 0 ? (long)Constantes.Area.SEM_AREA : bloqueio.CodArea,
                CodDispositivoBloqueio = bloqueio.CodDispositivoBloqueio == 0 ? (long)Constantes.DispositivoBloqueio.SEM_DISPOSITIVO_BLOQUEIO : bloqueio.CodDispositivoBloqueio,
                CodLocalABloquear = bloqueio.CodLocalABloquear == 0 ? (long)Constantes.LocalABloquear.SEM_LOCAL_A_BLOQUEAR : bloqueio.CodLocalABloquear,
                CodTagKKSBloqueio = bloqueio.CodTagKKSBloqueio == 0 ? (long)Constantes.TagKKsBloqueio.SEM_TAG_KKS_BLOQUEIO : bloqueio.CodTagKKSBloqueio,
                CodTipoEnergiaBloqueio = bloqueio.CodTipoEnergiaBloqueio == 0 ? (long)Constantes.TipoEnergiaBloqueio.SEM_TIPO_ENERGIA_BLOQUEIO : bloqueio.CodTipoEnergiaBloqueio,
            };

            entities.BLOQUEIO.Add(bloq);
            entities.SaveChanges();

            return bloq;

        }

        public BLOQUEIO_LOCAL_INSTALACAO InserirBloqueioLocalInstalacao(long codBloqueio, long codLocalInstalacao, string codigoBloqueioLocal,
            DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var bloqLI = new BLOQUEIO_LOCAL_INSTALACAO()
            {
                CodBloqueio = codBloqueio,
                CodLocalInstalacao = codLocalInstalacao,
                CodigoBloqueio = codigoBloqueioLocal
            };

            entities.BLOQUEIO_LOCAL_INSTALACAO.Add(bloqLI);
            entities.SaveChanges();

            return bloqLI;

        }

        public BLOQUEIO ListarBLoqueioPorCodigo(long codBloqueio, DB_LaborSafetyEntities entities = null)
        {

            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var bloqueioExistente = entities.BLOQUEIO.Where(x => x.CodBloqueio == codBloqueio)
                .FirstOrDefault();

            if (bloqueioExistente == null)
                throw new Exception($"O bloqueio informado é inválido!");

            return bloqueioExistente;
        }

        public BLOQUEIO EditaBloqueio(BLOQUEIO modelo, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            //Validação de modelo
            if (modelo.CodArea == 0)
                modelo.CodArea = (long)Constantes.Area.SEM_AREA;
            if (modelo.CodDispositivoBloqueio == 0)
                modelo.CodDispositivoBloqueio = (long)Constantes.DispositivoBloqueio.SEM_DISPOSITIVO_BLOQUEIO;
            if (modelo.CodLocalABloquear == 0)
                modelo.CodLocalABloquear = (long)Constantes.LocalABloquear.SEM_LOCAL_A_BLOQUEAR;
            if (modelo.CodTagKKSBloqueio == 0)
                modelo.CodTagKKSBloqueio = (long)Constantes.TagKKsBloqueio.SEM_TAG_KKS_BLOQUEIO;
            if (modelo.CodTipoEnergiaBloqueio == 0)
                modelo.CodTipoEnergiaBloqueio = (long)Constantes.TipoEnergiaBloqueio.SEM_TIPO_ENERGIA_BLOQUEIO;


            var bloqueioExistente = entities.BLOQUEIO.Where(x => x.CodBloqueio == modelo.CodBloqueio)
                .FirstOrDefault();

            if (bloqueioExistente == null)
                throw new Exception($"O bloqueio informado é inválido!");

            if (modelo.CodArea != (long)Constantes.Area.SEM_AREA)
                bloqueioExistente.CodArea = modelo.CodArea;

            if (modelo.CodDispositivoBloqueio != (long)Constantes.DispositivoBloqueio.SEM_DISPOSITIVO_BLOQUEIO)
                bloqueioExistente.CodDispositivoBloqueio = modelo.CodDispositivoBloqueio;

            if (modelo.CodLocalABloquear != (long)Constantes.LocalABloquear.SEM_LOCAL_A_BLOQUEAR)
                bloqueioExistente.CodLocalABloquear = modelo.CodLocalABloquear;

            if (modelo.CodTagKKSBloqueio != (long)Constantes.TagKKsBloqueio.SEM_TAG_KKS_BLOQUEIO)
                bloqueioExistente.CodTagKKSBloqueio = modelo.CodTagKKSBloqueio;

            if (modelo.CodTipoEnergiaBloqueio != (long)Constantes.TipoEnergiaBloqueio.SEM_TIPO_ENERGIA_BLOQUEIO)
                bloqueioExistente.CodTipoEnergiaBloqueio = modelo.CodTipoEnergiaBloqueio;

            entities.SaveChanges();

            return bloqueioExistente;

        }

        public BLOQUEIO_LOCAL_INSTALACAO ListarBloqueioLocalInstalacaoPorNomeELocal(string codigoBloqueioLocal, long codLocal,
            DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var bloqueio = entities.BLOQUEIO_LOCAL_INSTALACAO.Where(x => x.CodigoBloqueio.ToUpper() == codigoBloqueioLocal.ToUpper()
                                                                    && x.CodLocalInstalacao == codLocal).FirstOrDefault();
            return bloqueio;

        }

        public AREA InserirAreaBloqueio(AREA area, DB_LaborSafetyEntities entities = null)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var areaBloqueio = new AREA()
            {
                Codigo = area.Codigo,
                Nome = area.Nome,
                Descricao = area.Descricao
            };

            entities.AREA.Add(areaBloqueio);
            entities.SaveChanges();

            return areaBloqueio;

        }

        public AREA EditarAreaBloqueio(AREA area, DB_LaborSafetyEntities entities)
        {
            AREA areaExistente;

            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            areaExistente = entities.AREA.Where(ar => ar.CodArea == area.CodArea).FirstOrDefault();

            if (areaExistente == null)
                throw new KeyNotFoundException();

            areaExistente.Codigo = area.Codigo;
            areaExistente.Nome = area.Nome;

            entities.SaveChanges();

            return areaExistente;

        }

        public DISPOSITIVO_BLOQUEIO InserirDispositivoBloqueio(DISPOSITIVO_BLOQUEIO dispositivo, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var dispositivoBloqueio = new DISPOSITIVO_BLOQUEIO()
            {
                Codigo = dispositivo.Codigo,
                Nome = dispositivo.Nome
            };

            entities.DISPOSITIVO_BLOQUEIO.Add(dispositivoBloqueio);
            entities.SaveChanges();

            return dispositivoBloqueio;

        }

        public DISPOSITIVO_BLOQUEIO EditarDispositivoBloqueio(DISPOSITIVO_BLOQUEIO dispositivoBloqueio, DB_LaborSafetyEntities entities)
        {
            DISPOSITIVO_BLOQUEIO dispositivoBloqueioExistente;

            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            dispositivoBloqueioExistente = entities.DISPOSITIVO_BLOQUEIO.Where(dsp => dsp.CodDispositivoBloqueio
                == dispositivoBloqueio.CodDispositivoBloqueio).FirstOrDefault();

            if (dispositivoBloqueioExistente == null)
                throw new KeyNotFoundException();

            dispositivoBloqueioExistente.Codigo = dispositivoBloqueio.Codigo;
            dispositivoBloqueioExistente.Nome = dispositivoBloqueio.Nome;

            entities.SaveChanges();

            return dispositivoBloqueioExistente;

        }

        public TAG_KKS_BLOQUEIO InserirTagKKSBloqueio(TAG_KKS_BLOQUEIO tagKKSBloqueio, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var tagKKS = new TAG_KKS_BLOQUEIO()
            {
                Codigo = tagKKSBloqueio.Codigo,
                Nome = tagKKSBloqueio.Nome,
            };

            entities.TAG_KKS_BLOQUEIO.Add(tagKKS);
            entities.SaveChanges();

            return tagKKS;

        }

        public TAG_KKS_BLOQUEIO EditarTagKKS(TAG_KKS_BLOQUEIO tagKKSBloqueio, DB_LaborSafetyEntities entities)
        {
            TAG_KKS_BLOQUEIO tagKKSBloqueioExistente;

            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            tagKKSBloqueioExistente = entities.TAG_KKS_BLOQUEIO.Where(tag => tag.CodTagKKSBloqueio == tagKKSBloqueio.CodTagKKSBloqueio).FirstOrDefault();

            if (tagKKSBloqueioExistente == null)
                throw new KeyNotFoundException();

            tagKKSBloqueioExistente.Codigo = tagKKSBloqueio.Codigo;
            tagKKSBloqueioExistente.Nome = tagKKSBloqueio.Nome;

            entities.SaveChanges();

            return tagKKSBloqueioExistente;

        }

        public TIPO_ENERGIA_BLOQUEIO InserirTipoEnergia(TIPO_ENERGIA_BLOQUEIO tipoEnergiaBloqueio, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var tipoEnergia = new TIPO_ENERGIA_BLOQUEIO()
            {
                Codigo = tipoEnergiaBloqueio.Codigo,
                Nome = tipoEnergiaBloqueio.Nome,
            };

            entities.TIPO_ENERGIA_BLOQUEIO.Add(tipoEnergia);
            entities.SaveChanges();

            return tipoEnergia;

        }

        public TIPO_ENERGIA_BLOQUEIO EditarTipoEnergia(TIPO_ENERGIA_BLOQUEIO tipoEnergia, DB_LaborSafetyEntities entities)
        {
            TIPO_ENERGIA_BLOQUEIO tipoEnergiaExistente;

            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            tipoEnergiaExistente = entities.TIPO_ENERGIA_BLOQUEIO.Where(tpEnergia => tpEnergia.CodTipoEnergiaBloqueio
            == tipoEnergia.CodTipoEnergiaBloqueio).FirstOrDefault();

            if (tipoEnergiaExistente == null)
                throw new KeyNotFoundException();

            tipoEnergiaExistente.Codigo = tipoEnergia.Codigo;
            tipoEnergiaExistente.Nome = tipoEnergia.Nome;

            entities.SaveChanges();

            return tipoEnergiaExistente;

        }

        public LOCAL_A_BLOQUEAR InserirLocalABloquear(LOCAL_A_BLOQUEAR localABloquear, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var localBloqueio = new LOCAL_A_BLOQUEAR()
            {
                Nome = localABloquear.Nome,
            };

            entities.LOCAL_A_BLOQUEAR.Add(localBloqueio);
            entities.SaveChanges();

            return localBloqueio;

        }

        public LOCAL_A_BLOQUEAR EditarLocalABloquear(LOCAL_A_BLOQUEAR localABloquear, DB_LaborSafetyEntities entities)
        {
            LOCAL_A_BLOQUEAR localExistente;

            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            localExistente = entities.LOCAL_A_BLOQUEAR.Where(loc => loc.CodLocalABloquear == localABloquear.CodLocalABloquear).FirstOrDefault();

            if (localExistente == null)
                throw new KeyNotFoundException();

            localExistente.Nome = localABloquear.Nome;

            entities.SaveChanges();

            return localExistente;

        }

        public List<BLOQUEIO> ListarBloqueioPorListaLIPorArea(List<string> locais)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                List<long> CodLocais = new List<long>();

                foreach (var item in locais)
                {
                    var local = entities.LOCAL_INSTALACAO.Where(lc => lc.Nome == item).FirstOrDefault();
                    if (local != null)
                        CodLocais.Add(local.CodLocalInstalacao);
                    else
                        throw new Exception($"O local {item} não foi encontrado na base de dados!");
                }

                var bloqueio = entities.BLOQUEIO
                    .Include(x => x.BLOQUEIO_LOCAL_INSTALACAO)
                    .Include(x => x.DISPOSITIVO_BLOQUEIO)
                    .Include(x => x.TIPO_ENERGIA_BLOQUEIO)
                    .Include(x => x.TAG_KKS_BLOQUEIO)
                    .Include(x => x.LOCAL_A_BLOQUEAR)
                    .Include(x => x.AREA)
                    .Where(x => x.BLOQUEIO_LOCAL_INSTALACAO.Any(a => CodLocais.Contains(a.CodLocalInstalacao)))
                    .Select(x => x)
                    .ToList();

                return bloqueio;
            }
        }

        public List<BLOQUEIO> ListarBloqueioPorListaLIPorAreaLong(List<long> locais)
        {
            using (var entities = new DB_LaborSafetyEntities())
            {
                List<long> CodLocais = new List<long>();

                foreach (var item in locais)
                {
                    var local = entities.LOCAL_INSTALACAO.Where(lc => lc.CodLocalInstalacao == item).FirstOrDefault();
                    if (local != null)
                        CodLocais.Add(local.CodLocalInstalacao);
                    else
                        throw new Exception($"O local {item} não foi encontrado na base de dados!");
                }

                var bloqueio = entities.BLOQUEIO
                    .Include(x => x.BLOQUEIO_LOCAL_INSTALACAO)
                    .Include(x => x.DISPOSITIVO_BLOQUEIO)
                    .Include(x => x.TIPO_ENERGIA_BLOQUEIO)
                    .Include(x => x.TAG_KKS_BLOQUEIO)
                    .Include(x => x.LOCAL_A_BLOQUEAR)
                    .Include(x => x.AREA)
                    .Where(x => x.BLOQUEIO_LOCAL_INSTALACAO.Any(a => CodLocais.Contains(a.CodLocalInstalacao)))
                    .Select(x => x)
                    .ToList();

                return bloqueio;
            }
        }

        public TIPO_ENERGIA_BLOQUEIO ListarTipoEnergia(long idTipoEnergia, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var resultado = entities.TIPO_ENERGIA_BLOQUEIO.Where(x => x.CodTipoEnergiaBloqueio == idTipoEnergia).FirstOrDefault();
            return resultado;

        }

        public TAG_KKS_BLOQUEIO ListarTagKKS(long idTagKKS, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var resultado = entities.TAG_KKS_BLOQUEIO.Where(x => x.CodTagKKSBloqueio == idTagKKS).FirstOrDefault();
            return resultado;

        }

        public DISPOSITIVO_BLOQUEIO ListarDispositivo(long idDispositivo, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var resultado = entities.DISPOSITIVO_BLOQUEIO.Where(x => x.CodDispositivoBloqueio == idDispositivo).FirstOrDefault();
            return resultado;

        }

        public LOCAL_A_BLOQUEAR ListarLocalABloquear(long idLocalABloquear, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var resultado = entities.LOCAL_A_BLOQUEAR.Where(x => x.CodLocalABloquear == idLocalABloquear).FirstOrDefault();
            return resultado;

        }

        public AREA ListaAreaPorNome(string nome, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var resultado = entities.AREA.Where(x => x.Nome.ToUpper() == nome.ToUpper()).FirstOrDefault();
            return resultado;
        }

        public DISPOSITIVO_BLOQUEIO ListaDispositivoBloqueioNome(string nome, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var resultado = entities.DISPOSITIVO_BLOQUEIO.Where(x => x.Nome.ToUpper() == nome.ToUpper()).FirstOrDefault();
            return resultado;

        }

        public TAG_KKS_BLOQUEIO ListaTagKKSPorNome(string nome, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var resultado = entities.TAG_KKS_BLOQUEIO.Where(x => x.Nome.ToUpper() == nome.ToUpper()).FirstOrDefault();
            return resultado;

        }

        public TIPO_ENERGIA_BLOQUEIO ListaTipoEnergiaBloqueioNome(string nome, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            var resultado = entities.TIPO_ENERGIA_BLOQUEIO.Where(x => x.Nome.ToUpper() == nome.ToUpper()).FirstOrDefault();
            return resultado;
        }

        public LOCAL_A_BLOQUEAR ListaLocalABloquearPorNome(string nome, DB_LaborSafetyEntities entities)
        {
            if (entities == null)
                entities = new DB_LaborSafetyEntities();


            var resultado = entities.LOCAL_A_BLOQUEAR.Where(x => x.Nome.ToUpper() == nome.ToUpper()).FirstOrDefault();
            return resultado;

        }

        public void ApagarBloqueioLocal(long CodLocal, DB_LaborSafetyEntities entities = null)
        {
            BLOQUEIO_LOCAL_INSTALACAO bloqueioLocalExistente;

            if (entities == null)
                entities = new DB_LaborSafetyEntities();

            bloqueioLocalExistente = entities.BLOQUEIO_LOCAL_INSTALACAO.Where(blq => blq.CodLocalInstalacao == CodLocal).FirstOrDefault();

            if (bloqueioLocalExistente == null)
                return;

            entities.BLOQUEIO_LOCAL_INSTALACAO.Remove(bloqueioLocalExistente);
            entities.SaveChanges();
        }
    }
}
